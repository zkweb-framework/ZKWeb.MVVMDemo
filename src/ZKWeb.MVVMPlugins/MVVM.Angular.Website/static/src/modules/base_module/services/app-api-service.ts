import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptionsArgs } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/publishLast';
import { AppConfigService } from './app-config-service';

// 调用远程Api的服务
@Injectable()
export class AppApiService {
    // 全局Url过滤器
    private urlFilters: ((url: string) => string)[] = [];
    // 全局选项过滤器
    private optionsFilters: ((options: RequestOptionsArgs) => RequestOptionsArgs)[] = [];
    // 全局内容过滤器
    private bodyFilters: ((body: any) => any)[] = [];
    // 全局结果过滤器
    private resultFilters: ((response: Response) => Response)[] = [];
    // 全局错误过滤器
    private errorFilters: ((error: any) => any)[] = [];
    // 默认结果转换器
    private resultConverter: (response: Response) => any;
    // 默认错误转换器
    private errorConverter: (error: any) => Observable<any>;

    constructor(
        protected http: Http,
        protected appConfigService: AppConfigService) {
        // 设置http头
        this.registerOptionsFilter(options => {
            // 让服务端把请求当作ajax请求
            options.headers.append("X-Requested-With", "XMLHttpRequest");
            // 设置当前语言
            options.headers.append(this.appConfigService.getLanguageHeader(), this.appConfigService.getLanguage());
            // 设置当前时区
            options.headers.append(this.appConfigService.getTimezoneHeader(), this.appConfigService.getTimezone());
            // 附上会话Id
            options.headers.append(this.appConfigService.getSessionIdHeader(), this.appConfigService.getSessionId());
            return options;
        });
        // 如果内容包含文件对象则转换为FormData
        this.registerBodyFilter(body => {
            let formData = new FormData();
            // 设置最外层的参数
            for (let key in body) {
                if (body.hasOwnProperty(key)) {
                    formData.append(key, JSON.stringify(body[key]));
                }
            }
            // 枚举里层检测是否有文件对象
            let fileCount = 0;
            let visitor = (obj) => {
                // console.log("visit", obj);
                for (let key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        let value = obj[key];
                        if (value instanceof File) {
                            // 名称用原来的key，请注意重复
                            formData.append(key, value);
                            fileCount += 1;
                        } else if (value instanceof Object && value !== obj) {
                            // 检测子对象
                            visitor(value);
                        }
                    }
                }
            };
            visitor(body);
            // 有文件对象时返回FormData，否则返回原来的Body
            return (fileCount > 0) ? formData : body;
        });
        // 过滤回应
        this.registerResultFilter(response => {
            // 解析返回的会话Id
            let newSessionId = response.headers.get(this.appConfigService.getSessionIdSetHeader());
            if (newSessionId) {
                this.appConfigService.setSessionId(newSessionId);
            }
            return response;
        });
        // 设置默认的结果转换器
        this.setResultConverter(response => {
            try {
                // 尝试使用json解析
                return response.json();
            } catch (e) {
                // 失败时返回字符串
                return response.text();
            }
        });
        // 设置默认的错误转换器
        this.setErrorConverter(error => {
            console.log("api request error:", error);
            let errorMessage: string;
            if (error instanceof Response) {
                if (error.status === 0) {
                    // 网络错误时显示特殊信息
                    errorMessage = "Network error, please check your internet connection";
                } else {
                    // 返回过滤html标签后的文本
                    errorMessage = error.text().replace(/<[^>]+>/g, "");
                }
            } else {
                // 返回错误对象的json
                errorMessage = JSON.stringify(error);
            }
            return new Observable(o => {
                o.error(errorMessage);
                o.complete();
            });
        });
    }

    // 注册全局Url过滤器
    registerUrlFilter(filter: (string) => string) {
        this.urlFilters.push(filter);
    }

    // 注册全局选项过滤器
    registerOptionsFilter(filter: (options: RequestOptionsArgs) => RequestOptionsArgs) {
        this.optionsFilters.push(filter);
    }

    // 注册全局内容过滤器
    registerBodyFilter(filter: (body: any) => any) {
        this.bodyFilters.push(filter);
    }

    // 注册全局结果过滤器
    registerResultFilter(filter: (response: Response) => Response) {
        this.resultFilters.push(filter);
    }

    // 注册全局错误过滤器
    registerErrorFilter(filter: (error: any) => any) {
        this.errorFilters.push(filter);
    }

    // 设置默认结果转换器
    setResultConverter(converter: (response: Response) => any) {
        this.resultConverter = converter;
    }

    // 获取默认结果转换器
    getResultConverter(): (response: Response) => any {
        return this.resultConverter;
    }

    // 设置默认错误转换器
    setErrorConverter(converter: (error: any) => any) {
        this.errorConverter = converter;
    }

    // 获取默认错误转换器
    getErrorConverter(): (error: any) => any {
        return this.errorConverter;
    }

    // 调用Api函数
    call<T>(
        url: string,
        body: any,
        options?: RequestOptionsArgs,
        resultConverter?: (Response) => any,
        errorConverter?: (any) => any): Observable<T> {
        // 构建完整url，可能不在同一个host
        let fullUrl = this.appConfigService.getApiUrlBase() + url;
        this.urlFilters.forEach(h => { fullUrl = h(fullUrl); });
        // 构建选项，包括http头等
        options = options || { method: "POST" };
        options.headers = options.headers || new Headers();
        this.optionsFilters.forEach(h => { options = h(options); });
        // 构建提交内容
        this.bodyFilters.forEach(h => { body = h(body); });
        options.body = body;
        return this.http
            .request(fullUrl, options) // 提交api
            .publishLast().refCount() // 防止多次subscribe导致多次提交
            .map(response => {
                // 过滤返回的结果
                this.resultFilters.forEach(f => { response = f(response); });
                // 转换返回的结果
                return (resultConverter || this.resultConverter)(response);
            })
            .catch(error => {
                // 过滤返回的错误
                this.errorFilters.forEach(f => { error = f(error); });
                // 转换返回的错误
                return (errorConverter || this.errorConverter)(error);
            });
    }
}
