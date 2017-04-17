import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormGridComponent } from './components/form-grid.component';
import { FormValidationMessagesComponent } from './components/form-validation-messages.component';

@NgModule({
	imports: [
		CommonModule,
	],
	declarations: [
		FormGridComponent,
		FormValidationMessagesComponent
	],
	exports: [
		FormGridComponent,
		FormValidationMessagesComponent
	]
})
export class BaseFormModule { }
