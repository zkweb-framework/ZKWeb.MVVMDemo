# 前端如何调用API

### **调用生成的API**

后端会生成API脚本，调用时引用生成模块并注入相关服务即可调用，例如以下是关于网站的组件:

``` typescript
@Component({
    selector: 'admin-about-website',
    templateUrl: '../views/admin-about-website.html'
})
export class AdminAboutWebsiteComponent implements OnInit {
    language: string;
    timezone: string;
    apiUrlBase: string;
    websiteInfo: WebsiteInfoOutputDto;

    constructor(
        private appConfigService: AppConfigService,
        private appTranslationService: AppTranslationService,
        private websiteManagerService: WebsiteManageService,
        private adminToastService: AdminToastService) {
        this.websiteInfo = new WebsiteInfoOutputDto();
        this.websiteInfo.Plugins = [];
    }

    ngOnInit() {
        this.language = this.appTranslationService.translate(this.appConfigService.getLanguage());
        this.timezone = this.appTranslationService.translate(this.appConfigService.getTimezone());
        this.apiUrlBase = this.appConfigService.getApiUrlBase();
        this.websiteManagerService.GetWebsiteInfo().subscribe(
            s => this.websiteInfo = s,
            e => this.adminToastService.showToastMessage("error", e));
    }
}
```

这个组件调用了`WebsiteManageService.GetWebsiteInfo`服务，服务返回了`Observable<WebsiteInfoOutputDto>`

`Observable`是一个异步结果，你可以调用`subscribe`来指定成功和失败时的处理

关于`Observable`的更多信息可以查看[rxjs的文档](https://github.com/ReactiveX/RxJS)

### **AppApiService**

生成的代码内部都调用了`AppApiService`，这个类负责处理前端中所有的API请求

以下是生成的代码

``` typescript
constructor(private appApiService: AppApiService) { }

/** 获取网站信息 */
GetWebsiteInfo(): Observable<WebsiteInfoOutputDto> {
    return this.appApiService.call<WebsiteInfoOutputDto>(
        "/api/WebsiteManageService/GetWebsiteInfo",
        {
        });
}
```

`AppApiService.call`函数的参数如下

- url: API的URL
- body: 提交的内容，如果是个对象会自动提交成json，如果是FormGroup会自动提交成multipart
- options: 额外的选项，类型是`RequestOptionsArgs`
- resultConverter: 自定义的结果转换器，把回应转换到给subscribe处理的对象
- errorConverter: 自定义的错误转换器，把错误转换到给subscribe处理的对象

`AppApiService`类提供了丰富的接口实现全局处理API调用，包括

- registerUrlFilter(filter: (string) => string): 注册全局Url过滤器
- registerOptionsFilter(filter: (options: RequestOptionsArgs) => RequestOptionsArgs): 注册全局选项过滤器
- registerResultFilter(filter: (response: Response) => Response): 注册全局结果过滤器
- registerErrorFilter(filter: (error: any) => any): 注册全局错误过滤器
- setResultConverter(converter: (response: Response) => any): 设置默认结果转换器
- getResultConverter(): (response: Response) => any: 获取默认结果转换器
- setErrorConverter(converter: (error: any) => any): 设置默认错误转换器
- getErrorConverter(): (error: any) => any: 获取默认错误转换器

Url过滤器，选项过滤器，结果和错误过滤器可以注册多个，上一个过滤器的返回对象会交给下一个过滤器处理

结果和错误转换器只能注册一个，用于转换回应到给subscribe处理的对象

### **跨站请求**

Demo支持了跨站调用API，需要在index.html中设置`window.appConfig.apiUrlBase`

``` html
<script>
    window.appConfig = {
        apiUrlBase: "http://localhost:53128",
        // apiUrlBase: null
    };
</script>
```

设置后所有API请求都会以这个基础地址开头

注意跨站请求时服务端如果返回了额外的Http头给客户端，需要声明`Access-Control-Expose-Headers`客户端才能获取到

在Demo中则需要注册`ICORSExposeHeader`，这个接口的定义如下

``` csharp
/// <summary>
/// 允许跨站请求返回的头
/// </summary>
public interface ICORSExposeHeader
{
    string ExposeHeader { get; }
}
```
