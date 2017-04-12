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
var app_config_service_1 = require("./app-config-service");
var index_1 = require("../../generated_module/translations/index");
// 提供文本翻译的服务
var AppTranslationService = (function () {
    function AppTranslationService(appConfigService) {
        var _this = this;
        this.appConfigService = appConfigService;
        var language = appConfigService.getLanguage();
        index_1.TranslationIndex.translationModules.forEach(function (translation) {
            if (translation.language == language) {
                _this.translation = translation;
            }
        });
    }
    // 翻译指定文本，翻译不存在时返回原文本
    AppTranslationService.prototype.translate = function (text) {
        if (this.translation == null) {
            return text;
        }
        return this.translation.translations[text] || text;
    };
    return AppTranslationService;
}());
AppTranslationService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [app_config_service_1.AppConfigService])
], AppTranslationService);
exports.AppTranslationService = AppTranslationService;
