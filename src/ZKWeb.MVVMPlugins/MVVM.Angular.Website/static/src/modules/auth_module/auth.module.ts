import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';

import { AuthGuard } from './auth/auth-guard';
import { AppSessionService } from './services/app-session-service';

@NgModule({
	imports: [
		CommonModule,
		BaseModule,
		GeneratedModule
	],
	providers: [
		AuthGuard,
		AppSessionService
	]
})
export class AuthModule { }
