import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/publishLast';
import { AppConfigService } from './app-config-service';

// 调用远程Api的服务
@Injectable()
export class AppApiService {
	constructor(
		protected http: Http,
		protected appConfigService: AppConfigService) { }

    // 调用Api函数
	call<T>(url: string, args: {}): Observable<T> {
		var fullUrl = this.appConfigService.getApiUrlBase() + url;
		var body = JSON.stringify(args);
		return this.http
			.post(fullUrl, body)
			.publishLast().refCount() // 防止多次subscribe提交多次
			.map(this.extractData) // 解析返回的结果
			.catch(this.handleError); // 转换返回的错误
	}

	// 解析返回的结果
	protected extractData(response: Response) {
		return response.json();
	}

	// 转换返回的错误
	protected handleError(error: Response | any) {
		let errMsg: string;
		if (error instanceof Response) {
			const body = error.json() || '';
			const err = body.error || JSON.stringify(body);
			errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
		} else {
			errMsg = error.message ? error.message : error.toString();
		}
		console.error(errMsg);
		return Observable.throw(errMsg);
	}
}
