import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';

import { GeneratedModule } from '../generated_module/generated.module';
import { AdminModule } from '../admin_module/admin.module';

import { AppComponent } from './components/app.component';
import { PageNotFoundComponent } from './components/page_not_found.component';

import { AppApiService } from './services/app-api-service';
import { AppConfigService } from './services/app-config-service';
import { AppTranslationService } from './services/app-translation-service';

const routes: Routes = [
	{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
	imports: [
		BrowserModule,
		GeneratedModule,
		AdminModule,
		RouterModule.forRoot(routes)
	],
	declarations: [
		AppComponent,
		PageNotFoundComponent
	],
	providers: [
		AppApiService,
		AppConfigService,
		AppTranslationService
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
