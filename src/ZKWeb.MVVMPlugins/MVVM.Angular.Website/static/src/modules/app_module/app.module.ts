import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AdminModule } from '../admin_module/admin.module';

import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';

import { AppApiService } from './services/app-api-service';
import { AppConfigService } from './services/app-config-service';

const routes: Routes = [
	{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		AdminModule,
		RouterModule.forRoot(routes)
	],
	declarations: [
		AppComponent,
		PageNotFoundComponent
	],
	providers: [
		AppApiService,
		AppConfigService
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
