import { NgModule } from '@angular/core';
import { BaseModule } from '../base_module/base.module';
import { CaptchaService } from './services/captcha-service';
import { RoleManageService } from './services/role-manage-service';
import { SessionService } from './services/session-service';
import { TenantManageService } from './services/tenant-manage-service';
import { UserLoginService } from './services/user-login-service';
import { UserManageService } from './services/user-manage-service';
import { UserProfileService } from './services/user-profile-service';
import { WebsiteManageService } from './services/website-manage-service';
import { ExampleDataManageService } from './services/example-data-manage-service';

@NgModule({
	imports: [BaseModule],
	providers: [
		CaptchaService,
		RoleManageService,
		SessionService,
		TenantManageService,
		UserLoginService,
		UserManageService,
		UserProfileService,
		WebsiteManageService,
		ExampleDataManageService
	]
})
export class GeneratedModule { }
