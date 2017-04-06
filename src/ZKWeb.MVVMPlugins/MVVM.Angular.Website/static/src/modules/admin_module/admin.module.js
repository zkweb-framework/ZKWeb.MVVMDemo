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
var admin_container_component_1 = require("./components/admin-container.component");
var admin_index_component_1 = require("./components/admin-index.component");
var admin_login_component_1 = require("./components/admin-login.component");
var routes = [
    { path: '', redirectTo: '/admin', pathMatch: 'full' },
    { path: 'admin', component: admin_index_component_1.AdminIndexComponent },
    { path: 'admin/login', component: admin_login_component_1.AdminLoginComponent }
];
var AdminModule = (function () {
    function AdminModule() {
    }
    return AdminModule;
}());
AdminModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
            router_1.RouterModule.forRoot(routes)
        ],
        declarations: [
            admin_container_component_1.AdminContainerComponent,
            admin_index_component_1.AdminIndexComponent,
            admin_login_component_1.AdminLoginComponent
        ],
        exports: [
            router_1.RouterModule
        ]
    })
], AdminModule);
exports.AdminModule = AdminModule;
