import { NgModule } from '@angular/core';
import { BaseModule } from '../base_module/base.module';
import { CaptchaService } from './services/captcha-service';
import { SessionService } from './services/session-service';
import { UserLoginService } from './services/user-login-service';

@NgModule({
	imports: [BaseModule],
	providers: [
		CaptchaService,
		SessionService,
		UserLoginService
	]
})
export class GeneratedModule { }
