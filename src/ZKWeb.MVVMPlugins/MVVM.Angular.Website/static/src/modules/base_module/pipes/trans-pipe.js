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
var app_translation_service_1 = require("../../base_module/services/app-translation-service");
// 翻译文本的管道
// 例: {{ "text" | trans }}
var TransPipe = (function () {
    function TransPipe(appTranslationService) {
        this.appTranslationService = appTranslationService;
    }
    TransPipe.prototype.transform = function (value) {
        return this.appTranslationService.translate(value);
    };
    return TransPipe;
}());
TransPipe = __decorate([
    core_1.Pipe({ name: 'trans' }),
    __metadata("design:paramtypes", [app_translation_service_1.AppTranslationService])
], TransPipe);
exports.TransPipe = TransPipe;
