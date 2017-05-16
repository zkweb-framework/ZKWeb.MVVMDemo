import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
    selector: 'z-form-textarea',
    templateUrl: '../views/form-textarea.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormTextAreaComponent extends FormFieldBaseComponent {
    @Input() rows: number = 5;

    constructor(appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }
}
