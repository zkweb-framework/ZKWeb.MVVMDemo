import { Injectable } from '@angular/core';

// 保存全局配置的服务
@Injectable()
export class AppConfigService {
	private apiUrlBase: string;
	private language: string;
	private loginUrl: string[];
	private sessionIdKey: string;
	private sessionId: string;

	constructor() {
		var appConfig = window["appConfig"] || {};
		this.apiUrlBase = appConfig.apiUrlBase ||
			(location.protocol + "//" + location.host);
		this.language = appConfig.language || "en-US";
		this.loginUrl = appConfig.loginUrl || ["admin", "login"];
		this.sessionIdKey = appConfig.sessionIdKey || "ZKWeb-SessionId";
		// 从本地储存中获取会话Id，关闭浏览器再打开后也可以继续使用
		this.sessionId = localStorage.getItem(this.sessionIdKey);
	}

	// 获取Api的基础地址
	getApiUrlBase(): string {
		return this.apiUrlBase;
	}

	// 获取当前使用的语言
	getLanguage(): string {
		return this.language;
	}

	// 获取登录地址
	getLoginUrl(): string[] {
		return this.loginUrl;
	}

	// 获取当前会话Id
	getSessionId(): string {
		return this.sessionId;
	}

	// 设置当前会话Id
	setSessionId(sessionId: string): void {
		this.sessionId = sessionId;
		// 储存会话Id到本地储存
		localStorage.setItem(this.sessionIdKey, sessionId);
	}
}
