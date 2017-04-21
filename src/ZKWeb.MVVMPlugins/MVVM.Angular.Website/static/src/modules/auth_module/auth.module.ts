import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';

import { AuthGuard } from './auth/auth-guard';
import { AppPrivilegeService } from './services/app-privilege-service';
import { AppSessionService } from './services/app-session-service';

@NgModule({
	imports: [
		CommonModule,
		BaseModule,
		GeneratedModule
	],
	providers: [
		AuthGuard,
		AppPrivilegeService,
		AppSessionService
	]
})
export class AuthModule { }
