import { Injectable } from '@angular/core';

// 保存全局配置的服务
@Injectable()
export class AppConfigService {
	// Api的基础地址
	private apiUrlBase: string;
	// 当前使用的语言，例如"en-US"
	private language: string;
	// 客户端传给服务端使用的语言头
	private languageHeader: string;
	// 当前使用的时区，例如"America/Los_Angeles"
	private timezone: string;
	// 客户端传给服务端使用的语言头
	private timezoneHeader: string;
	// 登录地址
	private loginUrl: string[];
	// 当前会话Id
	private sessionId: string;
	// 客户端传给服务端使用的会话Id头
	private sessionIdHeader: string;
	// 服务端传给客户端使用的会话Id头
	private sessionIdSetHeader: string;
	// 会话Id储存在本地储存的key
	private sessionIdKey: string;

	constructor() {
		var appConfig = window["appConfig"] || {};
		this.apiUrlBase = appConfig.apiUrlBase ||
			(location.protocol + "//" + location.host);
		this.language = appConfig.language || "zh-CN";
		this.languageHeader = appConfig.languageHeader || "X-ZKWeb-Language";
		this.timezone = appConfig.timezone || "Asia/Shanghai";
		this.timezoneHeader = appConfig.timezoneHeader || "X-ZKWeb-Timezone";
		this.loginUrl = appConfig.loginUrl || ["/admin", "login"];
		this.sessionIdHeader = appConfig.sessionIdHeader || "X-ZKWeb-SessionId";
		this.sessionIdSetHeader = appConfig.sessionIdSetHeader || "X-Set-ZKWeb-SessionId";
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

	// 获取客户端传给服务端使用的语言头
	getLanguageHeader(): string {
		return this.languageHeader;
	}

	// 获取当前使用的时区
	getTimezone(): string {
		return this.timezone;
	}

	// 获取客户端传给服务端使用的语言头
	getTimezoneHeader(): string {
		return this.timezoneHeader;
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

	// 获取客户端传给服务端使用的会话Id头
	getSessionIdHeader(): string {
		return this.sessionIdHeader;
	}

	// 获取服务端传给客户端使用的会话Id头
	getSessionIdSetHeader(): string {
		return this.sessionIdSetHeader;
	}
}
