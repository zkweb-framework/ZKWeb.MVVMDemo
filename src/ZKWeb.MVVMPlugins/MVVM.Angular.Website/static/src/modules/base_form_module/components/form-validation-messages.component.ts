import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
	selector: 'z-form-validation-messages',
	templateUrl: '../views/form-validation-messages.html',
})
export class FormValidationMessagesComponent implements OnInit {
	@Input() formGroup: FormGroup;
	@Input() fieldName: string;
	@Input() displayName: string;
	errorMessages: string[] = [];

	constructor(private appTranslationService: AppTranslationService) {

	}

	private formatErrorMessage(type: string, data: any): string {
		console.log("format error message", type, data);
		return type;
	}

	ngOnInit() {
		var formControl = this.formGroup.controls[this.fieldName];
		if (formControl == null) {
			throw `formControl '${this.fieldName}' not exist`;
		}
		// 监听控件状态改变
		formControl.statusChanges.subscribe(_ => {
			if (formControl.valid || !formControl.dirty) {
				this.errorMessages = [];
				return;
			}
			// 控件已修改且验证不通过时显示错误
			this.errorMessages = [];
			var errors = formControl.errors;
			for (var key in errors) {
				if (errors.hasOwnProperty(key)) {
					this.errorMessages.push(this.formatErrorMessage(key, errors[key]));
				}
			}
		});
	}
}
