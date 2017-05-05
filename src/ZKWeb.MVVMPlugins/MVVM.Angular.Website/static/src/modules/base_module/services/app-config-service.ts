import { Injectable } from '@angular/core';

/** 保存全局配置的服务 */
@Injectable()
export class AppConfigService {
    /** Api的基础地址 */
    private apiUrlBase: string;
    /** 当前使用的语言，例如"en-US" */
    private language: string;
    /** 默认使用的语言 */
    private defaultLanguage: string;
    /** 客户端传给服务端使用的语言头 */
    private languageHeader: string;
    /** 当前使用的语言在本地储存的key */
    private languageKey: string;
    /** 当前使用的时区，例如"America/Los_Angeles" */
    private timezone: string;
    /** 默认使用的时区 */
    private defaultTimezone: string;
    /** 客户端传给服务端使用的语言头 */
    private timezoneHeader: string;
    /** 当前使用的时区在本地储存的key */
    private timezoneKey: string;
    /** 登录地址 */
    private loginUrl: string[];
    /** 当前会话Id */
    private sessionId: string;
    /** 客户端传给服务端使用的会话Id头 */
    private sessionIdHeader: string;
    /** 服务端传给客户端使用的会话Id头 */
    private sessionIdSetHeader: string;
    /** 会话Id储存在本地储存的key */
    private sessionIdKey: string;

    constructor() {
        let appConfig = window["appConfig"] || {};
        this.apiUrlBase = appConfig.apiUrlBase ||
            (location.protocol + "//" + location.host);
        this.language = appConfig.language || null;
        this.defaultLanguage = appConfig.defaultLnaguage || "zh-CN";
        this.languageHeader = appConfig.languageHeader || "X-ZKWeb-Language";
        this.languageKey = appConfig.languageKey || "ZKWeb-Language";
        this.timezone = appConfig.timezone || null;
        this.defaultTimezone = appConfig.defaultTimezone || "Asia/Shanghai";
        this.timezoneHeader = appConfig.timezoneHeader || "X-ZKWeb-Timezone";
        this.timezoneKey = appConfig.timezoneKey || "ZKWeb-Timezone";
        this.loginUrl = appConfig.loginUrl || ["/admin", "login"];
        this.sessionIdHeader = appConfig.sessionIdHeader || "X-ZKWeb-SessionId";
        this.sessionIdSetHeader = appConfig.sessionIdSetHeader || "X-Set-ZKWeb-SessionId";
        this.sessionIdKey = appConfig.sessionIdKey || "ZKWeb-SessionId";
    }

    /** 获取Api的基础地址 */
    getApiUrlBase(): string {
        return this.apiUrlBase;
    }

    /** 获取当前使用的语言 */
    getLanguage(): string {
        if (!this.language) {
            this.language = localStorage.getItem(this.languageKey) || this.defaultLanguage;
        }
        return this.language;
    }

    /** 设置当前使用的语言 */
    setLanguage(language: string) {
        this.language = language;
        localStorage.setItem(this.languageKey, language);
    }

    // 获取客户端传给服务端使用的语言头
    getLanguageHeader(): string {
        return this.languageHeader;
    }

    // 获取当前使用的时区
    getTimezone(): string {
        if (!this.timezone) {
            this.timezone = localStorage.getItem(this.timezone) || this.defaultTimezone;
        }
        return this.timezone;
    }

    /** 设置当前使用的时区 */
    setTimezone(timezone: string) {
        this.timezone = timezone;
        localStorage.setItem(this.timezoneKey, timezone);
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
        if (!this.sessionId) {
            this.sessionId = localStorage.getItem(this.sessionIdKey);
        }
        return this.sessionId;
    }

    // 设置当前会话Id
    setSessionId(sessionId: string): void {
        this.sessionId = sessionId;
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
