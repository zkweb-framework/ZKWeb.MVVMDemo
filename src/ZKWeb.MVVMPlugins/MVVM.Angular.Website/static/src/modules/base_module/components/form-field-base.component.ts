import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

export abstract class FormFieldBaseComponent implements OnInit {
	/** 表单对象 */
	@Input() formGroup: FormGroup;
	/** 字段名称 */
	@Input() fieldName: string;
	/** 显示名称，省略时用字段名称 */
	@Input() displayName: string;
	/** 预置文本，省略时用显示名称 */
	@Input() placeHolder: string;
	/** 验证信息，省略时用默认的验证信息 */
	@Input() validationMessages: { [key: string]: string };
	/** 文本的网格宽度 */
	@Input() labelGridWidth: number = 4;
	/** 控件的网格宽度 */
	@Input() fieldGridWidth: number = 8;
	/** 翻译后的显示名称 */
	translatedDisplayName: string;
	/** 翻译后的预置文本 */
	translatedPlaceHolder: string;

	constructor(protected appTranslationService: AppTranslationService) {

	}

	ngOnInit() {
		this.translatedDisplayName = this.appTranslationService.translate(this.displayName || this.fieldName);
		this.translatedPlaceHolder = this.appTranslationService.translate(this.placeHolder) || this.translatedDisplayName;
	}
}
