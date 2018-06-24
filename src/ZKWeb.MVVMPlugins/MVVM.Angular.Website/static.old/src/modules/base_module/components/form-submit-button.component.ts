import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
    selector: 'z-form-submit-button',
    templateUrl: '../views/form-submit-button.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormSubmitButtonComponent implements OnInit {
    @Input() formGroup: FormGroup;
    @Input() displayName: string;
    @Input() leftGridWidth: number = 0;
    @Input() selfGridWidth: number = 12;
    @Input() isSubmitting: any;
    translatedDisplayName: string;

    constructor(private appTranslationService: AppTranslationService) {

    }

    ngOnInit() {
        this.translatedDisplayName = this.appTranslationService.translate(this.displayName || "Submit");
    }
}
