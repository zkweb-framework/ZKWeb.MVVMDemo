import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { BaseModule } from '../base_module/base.module';

import { AdminContainerComponent } from './components/admin-container.component';
import { AdminIndexComponent } from './components/admin-index.component';
import { AdminLoginComponent } from './components/admin-login.component';

import { AdminAuthGuard } from './auth/admin-auth-guard';

const routes: Routes = [
	{ path: '', redirectTo: '/admin', pathMatch: 'full' },
	{ path: 'admin', component: AdminIndexComponent, canActivate: [AdminAuthGuard] },
	{ path: 'admin/login', component: AdminLoginComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		BaseModule,
		RouterModule.forChild(routes)
	],
	declarations: [
		AdminContainerComponent,
		AdminIndexComponent,
		AdminLoginComponent
	],
	providers: [
		AdminAuthGuard
	],
	exports: [
		RouterModule
	]
})
export class AdminModule { }
