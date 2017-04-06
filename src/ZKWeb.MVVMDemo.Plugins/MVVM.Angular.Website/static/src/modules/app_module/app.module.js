"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var app_component_1 = require("./components/app.component");
var main_sidebar_component_1 = require("./components/main-sidebar.component");
var control_sidebar_component_1 = require("./components/control-sidebar.component");
var header_component_1 = require("./components/header.component");
var home_component_1 = require("./components/home.component");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule
        ],
        declarations: [
            app_component_1.AppComponent,
            main_sidebar_component_1.MainSidebarComponent,
            control_sidebar_component_1.ControlSidebarComponent,
            header_component_1.HeaderComponent,
            home_component_1.HomeComponent
        ],
        bootstrap: [
            app_component_1.AppComponent
        ]
    })
], AppModule);
exports.AppModule = AppModule;
