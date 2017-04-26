import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule, InputTextareaModule, ButtonModule } from 'primeng/primeng';

import { FormGridComponent } from './components/form-grid.component';
import { FormValidationMessagesComponent } from './components/form-validation-messages.component';
import { FormTextComponent } from './components/form-text.component';
import { FormTextAreaComponent } from './components/form-textarea.component';
import { FormHiddenComponent } from './components/form-hidden.component';
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
		HttpModule,
		FormsModule,
		ReactiveFormsModule,
		InputTextModule,
		InputTextareaModule,
		ButtonModule
	],
	declarations: [
		FormGridComponent,
		FormValidationMessagesComponent,
		FormTextComponent,
		FormTextAreaComponent,
		FormHiddenComponent,
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
		FormTextAreaComponent,
		FormHiddenComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormSubmitButtonComponent,
		TransPipe
	]
})
export class BaseModule { }
