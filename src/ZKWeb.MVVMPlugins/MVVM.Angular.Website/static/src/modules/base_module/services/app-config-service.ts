import { Injectable } from '@angular/core';

// 保存全局配置的服务
@Injectable()
export class AppConfigService {
	private apiUrlBase: string;
	private language: string;

	constructor() {
		var appConfig = window["appConfig"] || {};
		this.apiUrlBase = appConfig.apiUrlBase ||
			(location.protocol + "//" + location.host);
		this.language = appConfig.language || "en-US";
	}

	// 获取Api的基础地址
	getApiUrlBase(): string {
		return this.apiUrlBase;
	}

	// 获取当前使用的语言
	getLanguage(): string {
		return this.language;
	}
}
