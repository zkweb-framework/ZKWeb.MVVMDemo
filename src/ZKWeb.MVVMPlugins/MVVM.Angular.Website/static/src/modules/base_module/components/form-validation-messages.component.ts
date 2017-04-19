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
	@Input() validationMessages: { [key: string]: string };
	errorMessages: string[] = [];

	constructor(private appTranslationService: AppTranslationService) {

	}

	private formatErrorMessage(type: string, data: any): string {
		// 获取自定义的错误信息
		var message = (this.validationMessages || {})[type];
		if (message) {
			return message;
		}
		// 构建默认的错误信息
		var fieldDisplayName = this.appTranslationService.translate(this.displayName || this.fieldName);
		if (type == "required") {
			message = this.appTranslationService.translate("{0} is required")
				.replace("{0}", fieldDisplayName);
		} else if (type == "minlength") {
			message = this.appTranslationService.translate("Length of {0} must not less than {1}")
				.replace("{0}", fieldDisplayName)
				.replace("{1}", data.requiredLength);
		} else if (type == "maxlength") {
			message = this.appTranslationService.translate("Length of {0} must not greater than {1}")
				.replace("{0}", fieldDisplayName)
				.replace("{1}", data.requiredLength);
		} else if (type == "email" || type == "pattern") {
			message = this.appTranslationService.translate("Format of {0} is incorrect")
				.replace("{0}", fieldDisplayName);
		} else {
			message = type;
		}
		return message;
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
