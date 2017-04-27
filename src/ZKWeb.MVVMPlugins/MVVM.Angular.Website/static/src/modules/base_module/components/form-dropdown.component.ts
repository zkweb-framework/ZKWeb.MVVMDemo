import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { SelectItem } from 'primeng/primeng';

@Component({
	selector: 'z-form-dropdown',
	templateUrl: '../views/form-dropdown.html',
	host: { 'class': 'ui-grid-row' }
})
export class FormDropdownComponent extends FormFieldBaseComponent {
	@Input() options: SelectItem[];
}
