"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var router_1 = require("@angular/router");
var generated_module_1 = require("../generated_module/generated.module");
var admin_module_1 = require("../admin_module/admin.module");
var app_component_1 = require("./components/app.component");
var page_not_found_component_1 = require("./components/page_not_found.component");
var app_api_service_1 = require("./services/app-api-service");
var app_config_service_1 = require("./services/app-config-service");
var app_translation_service_1 = require("./services/app-translation-service");
var routes = [
    { path: '**', component: page_not_found_component_1.PageNotFoundComponent }
];
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
            generated_module_1.GeneratedModule,
            admin_module_1.AdminModule,
            router_1.RouterModule.forRoot(routes)
        ],
        declarations: [
            app_component_1.AppComponent,
            page_not_found_component_1.PageNotFoundComponent
        ],
        providers: [
            app_api_service_1.AppApiService,
            app_config_service_1.AppConfigService,
            app_translation_service_1.AppTranslationService
        ],
        bootstrap: [
            app_component_1.AppComponent
        ]
    })
], AppModule);
exports.AppModule = AppModule;
