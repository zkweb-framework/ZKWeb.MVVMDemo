"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var trans_pipe_1 = require("./pipes/trans-pipe");
var app_api_service_1 = require("./services/app-api-service");
var app_config_service_1 = require("./services/app-config-service");
var app_translation_service_1 = require("./services/app-translation-service");
var BaseModule = (function () {
    function BaseModule() {
    }
    return BaseModule;
}());
BaseModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
        ],
        declarations: [
            trans_pipe_1.TransPipe
        ],
        providers: [
            app_api_service_1.AppApiService,
            app_config_service_1.AppConfigService,
            app_translation_service_1.AppTranslationService
        ],
        exports: [
            trans_pipe_1.TransPipe
        ]
    })
], BaseModule);
exports.BaseModule = BaseModule;
