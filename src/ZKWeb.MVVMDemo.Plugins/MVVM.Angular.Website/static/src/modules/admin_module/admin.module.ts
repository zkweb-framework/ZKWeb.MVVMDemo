import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';

import { AdminIndexComponent } from './components/admin-index.component';

const routes: Routes = [
	{ path: '', redirectTo: '/admin', pathMatch: 'full' },
	{ path: 'admin', component: AdminIndexComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		RouterModule.forRoot(routes)
	],
	declarations: [
		AdminIndexComponent
	],
	exports: [
		RouterModule
	]
})
export class AdminModule { }
