import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';

@Component({
    selector: 'z-form-textarea',
    templateUrl: '../views/form-textarea.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormTextAreaComponent extends FormFieldBaseComponent {
    @Input() rows: number = 5;
}
