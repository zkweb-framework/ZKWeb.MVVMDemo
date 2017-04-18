import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

export abstract class FormFieldBaseComponent implements OnInit {
	@Input() formGroup: FormGroup;
	@Input() fieldName: string;
	@Input() displayName: string;
	@Input() placeHolder: string;
	@Input() validationMessages: { [key: string]: string };
	@Input() labelGridWidth: number = 4;
	@Input() fieldGridWidth: number = 8;
	translatedDisplayName: string;
	translatedPlaceHolder: string;

	constructor(private appTranslationService: AppTranslationService) {

	}

	ngOnInit() {
		this.translatedDisplayName = this.appTranslationService.translate(this.displayName || this.fieldName);
		this.translatedPlaceHolder = this.appTranslationService.translate(this.placeHolder || this.fieldName);
	}
}
