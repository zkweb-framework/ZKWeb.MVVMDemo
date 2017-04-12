import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { TransPipe } from './pipes/trans-pipe';

import { AppApiService } from './services/app-api-service';
import { AppConfigService } from './services/app-config-service';
import { AppTranslationService } from './services/app-translation-service';

@NgModule({
	imports: [
		BrowserModule,
	],
	declarations: [
		TransPipe
	],
	providers: [
		AppApiService,
		AppConfigService,
		AppTranslationService
	],
	exports: [
		TransPipe
	]
})
export class BaseModule { }
