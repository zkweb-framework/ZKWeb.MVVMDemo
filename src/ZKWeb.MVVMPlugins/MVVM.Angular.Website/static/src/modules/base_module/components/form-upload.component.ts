import { Component } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';

@Component({
	selector: 'z-form-upload',
	templateUrl: '../views/form-upload.html',
	host: { 'class': 'ui-grid-row' }
})
export class FormUploadComponent extends FormFieldBaseComponent {
	/** 把File对象设置到FormControl中，如果用默认的formControlName只会设置文件名 */
	onFileChange(event) {
		let files: File[] = event.target.files;
		if (files.length > 0) {
			let formControl = this.formGroup.controls[this.fieldName];
			if (formControl == null) {
				throw `formControl '${this.fieldName}' not exist`;
			}
			formControl.setValue(files[0]);
		}
	}
}
