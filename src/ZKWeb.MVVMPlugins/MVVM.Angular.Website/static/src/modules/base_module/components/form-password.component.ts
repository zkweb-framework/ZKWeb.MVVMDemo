import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';

@Component({
    selector: 'z-form-password',
    templateUrl: '../views/form-password.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormPasswordComponent extends FormFieldBaseComponent { }
