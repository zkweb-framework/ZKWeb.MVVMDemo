import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';

import { GeneratedModule } from '../generated_module/generated.module';
import { BaseModule } from '../base_module/base.module';
import { AdminModule } from '../admin_module/admin.module';

import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';

const routes: Routes = [
	// 404页面
	{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		GeneratedModule,
		BaseModule,
		AdminModule,
		RouterModule.forRoot(routes)
	],
	declarations: [
		AppComponent,
		PageNotFoundComponent
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
