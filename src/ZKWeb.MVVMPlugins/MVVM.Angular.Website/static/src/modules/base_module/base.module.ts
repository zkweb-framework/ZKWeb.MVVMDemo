import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminAuthGuard } from './auth/admin-auth-guard';
import { TransPipe } from './pipes/trans-pipe';

import { AppApiService } from './services/app-api-service';
import { AppConfigService } from './services/app-config-service';
import { AppTranslationService } from './services/app-translation-service';

@NgModule({
	imports: [
		CommonModule,
	],
	declarations: [
		TransPipe
	],
	providers: [
		AdminAuthGuard,
		AppApiService,
		AppConfigService,
		AppTranslationService
	],
	exports: [
		TransPipe
	]
})
export class BaseModule { }
