import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';

@Component({
	selector: 'z-form-captcha',
	templateUrl: '../views/form-captcha.html',
	host: { 'class': 'ui-grid-row' }
})
export class FormCaptchaComponent extends FormFieldBaseComponent { }
