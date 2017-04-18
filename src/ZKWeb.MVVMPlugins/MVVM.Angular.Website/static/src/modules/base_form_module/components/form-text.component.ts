import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';

@Component({
	selector: 'z-form-text',
	templateUrl: '../views/form-text.html',
	host: { 'class': 'ui-grid-row' }
})
export class FormTextComponent extends FormFieldBaseComponent { }
