import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';

import { AdminContainerComponent } from './components/admin-container.component';
import { AdminIndexComponent } from './components/admin-index.component';
import { AdminLoginComponent } from './components/admin-login.component';

const routes: Routes = [
	{ path: '', redirectTo: '/admin', pathMatch: 'full' },
	{ path: 'admin', component: AdminIndexComponent },
	{ path: 'admin/login', component: AdminLoginComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		RouterModule.forRoot(routes)
	],
	declarations: [
		AdminContainerComponent,
		AdminIndexComponent,
		AdminLoginComponent
	],
	exports: [
		RouterModule
	]
})
export class AdminModule { }
