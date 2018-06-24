import { Component, Input } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { SelectItem } from 'primeng/components/common/api';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
    selector: 'z-form-multiselect',
    templateUrl: '../views/form-multiselect.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormMultiSelectComponent extends FormFieldBaseComponent {
    @Input() options: SelectItem[];
    @Input() defaultLabel: null;

    constructor(appTranslationService: AppTranslationService) {
        super(appTranslationService);
    }
}
