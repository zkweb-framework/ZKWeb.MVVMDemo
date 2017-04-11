import { Injectable } from '@angular/core';

// 保存全局配置的服务
@Injectable()
export class AppConfigService {
	private apiUrlBase: string;

	constructor() {
		var overrideConfig = window["overrideConfig"] || {};
		this.apiUrlBase = overrideConfig.apiUrlBase;
		if (!this.apiUrlBase) {
			this.apiUrlBase = location.protocol + "//" + location.host + "/api/";
		}
	}

	// 获取Api的基础地址
	// 默认和当前浏览器的地址一样，有需要可以修改window.overrideConfig.apiUrlBase
	getApiUrlBase(): string {
		return this.apiUrlBase;
	}
}
