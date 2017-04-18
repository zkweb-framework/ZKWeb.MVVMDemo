import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule, ButtonModule } from 'primeng/primeng';

import { FormGridComponent } from './components/form-grid.component';
import { FormValidationMessagesComponent } from './components/form-validation-messages.component';
import { FormTextComponent } from './components/form-text.component';
import { FormPasswordComponent } from './components/form-password.component';
import { FormCaptchaComponent } from './components/form-captcha.component';
import { FormSubmitButtonComponent } from './components/form-submit-button.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		InputTextModule,
		ButtonModule,
	],
	declarations: [
		FormGridComponent,
		FormValidationMessagesComponent,
		FormTextComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormSubmitButtonComponent
	],
	exports: [
		FormGridComponent,
		FormValidationMessagesComponent,
		FormTextComponent,
		FormPasswordComponent,
		FormCaptchaComponent,
		FormSubmitButtonComponent
	]
})
export class BaseFormModule { }
