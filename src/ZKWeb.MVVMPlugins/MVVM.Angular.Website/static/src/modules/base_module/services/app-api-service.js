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
var http_1 = require("@angular/http");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/catch");
require("rxjs/add/operator/map");
var app_config_service_1 = require("./app-config-service");
// 调用远程Api的服务
var AppApiService = (function () {
    function AppApiService(http, appConfigService) {
        this.http = http;
        this.appConfigService = appConfigService;
    }
    // 调用Api函数
    AppApiService.prototype.call = function (url, args) {
        var fullUrl = this.appConfigService.getApiUrlBase() + url;
        var body = JSON.stringify(args);
        return this.http
            .post(fullUrl, body)
            .map(this.extractData)
            .catch(this.handleError);
    };
    // 解析返回的结果
    AppApiService.prototype.extractData = function (response) {
        return response.json();
    };
    // 处理返回的错误
    AppApiService.prototype.handleError = function (error) {
        var errMsg;
        if (error instanceof http_1.Response) {
            var body = error.json() || '';
            var err = body.error || JSON.stringify(body);
            errMsg = error.status + " - " + (error.statusText || '') + " " + err;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable_1.Observable.throw(errMsg);
    };
    return AppApiService;
}());
AppApiService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http,
        app_config_service_1.AppConfigService])
], AppApiService);
exports.AppApiService = AppApiService;
