import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
	InputTextModule,
	InputTextareaModule,
	DropdownModule,
	MultiSelectModule,
	ButtonModule
} from 'primeng/primeng';

import { FormGridComponent } from './components/form-grid.component';
import { FormValidationMessagesComponent } from './components/form-validation-messages.component';
import { FormTextComponent } from './components/form-text.component';
import { FormTextAreaComponent } from './components/form-textarea.component';
import { FormDropdownComponent } from './components/form-dropdown.component';
import { FormMultiSelectComponent } from './components/form-multiselect.component';
import { FormHiddenComponent } from './components/form-hidden.component';
import { FormPasswordComponent } from './components/form-password.component';
import { FormCaptchaComponent } from './components/form-captcha.component';
import { FormUploadComponent } from './components/form-upload.component';

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
		DropdownModule,
		MultiSelectModule,
		ButtonModule
	],
	declarations: [
		FormGridComponent,
		FormValidationMessagesComponent,
		FormTextComponent,
		FormTextAreaComponent,
		FormDropdownComponent,
		FormMultiSelectComponent,
		FormHiddenComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormUploadComponent,
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
		FormDropdownComponent,
		FormMultiSelectComponent,
		FormHiddenComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormUploadComponent,
		FormSubmitButtonComponent,
		TransPipe
	]
})
export class BaseModule { }
