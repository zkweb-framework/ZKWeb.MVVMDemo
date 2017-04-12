"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
// 保存全局配置的服务
var AppConfigService = (function () {
    function AppConfigService() {
        var appConfig = window["appConfig"] || {};
        this.apiUrlBase = appConfig.apiUrlBase ||
            (location.protocol + "//" + location.host);
        this.language = appConfig.language || "en-US";
    }
    // 获取Api的基础地址
    AppConfigService.prototype.getApiUrlBase = function () {
        return this.apiUrlBase;
    };
    // 获取当前使用的语言
    AppConfigService.prototype.getLanguage = function () {
        return this.language;
    };
    return AppConfigService;
}());
AppConfigService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [])
], AppConfigService);
exports.AppConfigService = AppConfigService;
