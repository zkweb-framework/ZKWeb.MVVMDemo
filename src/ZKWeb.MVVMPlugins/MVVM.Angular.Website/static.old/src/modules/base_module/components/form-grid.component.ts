import { Component } from '@angular/core';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
    selector: 'z-form-grid',
    templateUrl: '../views/form-grid.html',
    host: { 'class': 'ui-grid ui-grid-responsive ui-grid-pad ui-fluid' }
})
export class FormGridComponent { }
