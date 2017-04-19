import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule, ButtonModule } from 'primeng/primeng';

import { FormGridComponent } from './components/form-grid.component';
import { FormValidationMessagesComponent } from './components/form-validation-messages.component';
import { FormTextComponent } from './components/form-text.component';
import { FormPasswordComponent } from './components/form-password.component';
import { FormCaptchaComponent } from './components/form-captcha.component';
import { FormSubmitButtonComponent } from './components/form-submit-button.component';
import { TransPipe } from './pipes/trans-pipe';

import { AppApiService } from './services/app-api-service';
import { AppConfigService } from './services/app-config-service';
import { AppTranslationService } from './services/app-translation-service';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		InputTextModule,
		ButtonModule
	],
	declarations: [
		FormGridComponent,
		FormValidationMessagesComponent,
		FormTextComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormSubmitButtonComponent,
		TransPipe
	],
	providers: [
		AppApiService,
		AppConfigService,
		AppTranslationService
	],
	exports: [
		FormGridComponent,
		FormValidationMessagesComponent,
		FormTextComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormSubmitButtonComponent,
		TransPipe
	]
})
export class BaseModule { }
