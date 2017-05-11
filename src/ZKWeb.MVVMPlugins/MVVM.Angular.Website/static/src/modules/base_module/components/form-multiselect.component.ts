import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { SelectItem } from 'primeng/components/common/api';

@Component({
    selector: 'z-form-multiselect',
    templateUrl: '../views/form-multiselect.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormMultiSelectComponent extends FormFieldBaseComponent {
    @Input() options: SelectItem[];
    @Input() defaultLabel: null;
}
