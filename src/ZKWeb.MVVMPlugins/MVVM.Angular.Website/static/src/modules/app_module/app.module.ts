import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';

import { GeneratedModule } from '../generated_module/generated.module';
import { BaseModule } from '../base_module/base.module';

import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';

const routes: Routes = [
	{ path: '', redirectTo: '/admin', pathMatch: 'full' },
	{ path: 'admin', loadChildren: '../admin_module/admin.module#AdminModule' },
	{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		GeneratedModule,
		BaseModule,
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
