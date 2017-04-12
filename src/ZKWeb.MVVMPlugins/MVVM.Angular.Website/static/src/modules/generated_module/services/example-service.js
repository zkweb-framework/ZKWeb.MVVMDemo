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
var app_api_service_1 = require("../../base_module/services/app-api-service");
var ExampleService = (function () {
    function ExampleService(appApiService) {
        this.appApiService = appApiService;
    }
    // 获取对象
    ExampleService.prototype.GetObject = function (x) {
        return this.appApiService.call("/api/ExampleService/GetObject", {
            x: x
        });
    };
    // 获取Dto
    ExampleService.prototype.GetDto = function (param, a) {
        return this.appApiService.call("/api/ExampleService/GetDto", {
            param: param,
            a: a
        });
    };
    return ExampleService;
}());
ExampleService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [app_api_service_1.AppApiService])
], ExampleService);
exports.ExampleService = ExampleService;
