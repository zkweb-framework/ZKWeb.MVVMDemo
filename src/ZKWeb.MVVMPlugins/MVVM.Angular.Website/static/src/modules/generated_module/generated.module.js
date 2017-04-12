"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var test_dto_1 = require("./dtos/test-dto");
var test_other_dto_1 = require("./dtos/test-other-dto");
var test_param_1 = require("./dtos/test-param");
var example_service_1 = require("./services/example-service");
var zh_cn_1 = require("./translations/zh-cn");
var en_us_1 = require("./translations/en-us");
var GeneratedModule = (function () {
    function GeneratedModule() {
    }
    return GeneratedModule;
}());
GeneratedModule.translationModules = [
    zh_cn_1.Translation_zh_CN,
    en_us_1.Translation_en_US
];
GeneratedModule = __decorate([
    core_1.NgModule({
        providers: [
            test_dto_1.TestDto,
            test_other_dto_1.TestOtherDto,
            test_param_1.TestParam,
            example_service_1.ExampleService,
            zh_cn_1.Translation_zh_CN,
            en_us_1.Translation_en_US
        ]
    })
], GeneratedModule);
exports.GeneratedModule = GeneratedModule;
