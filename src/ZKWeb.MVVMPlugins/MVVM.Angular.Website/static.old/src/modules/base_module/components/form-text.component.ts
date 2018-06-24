import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
    selector: 'z-form-text',
    templateUrl: '../views/form-text.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormTextComponent extends FormFieldBaseComponent {
    constructor(appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }
}
