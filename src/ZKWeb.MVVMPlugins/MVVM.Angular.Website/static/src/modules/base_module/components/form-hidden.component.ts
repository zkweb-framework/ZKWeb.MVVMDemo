import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';

@Component({
	selector: 'z-form-hidden',
	templateUrl: '../views/form-hidden.html',
	host: { 'class': 'ui-grid-row' }
})
export class FormHiddenComponent extends FormFieldBaseComponent { }
