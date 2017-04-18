import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Message } from 'primeng/primeng';

@Component({
	selector: 'admin-login',
	templateUrl: '../views/admin-login.html',
	styleUrls: ['../styles/admin-login.scss']
})
export class AdminLoginComponent implements OnInit {
	adminLoginForm = new FormGroup({
		Tenant: new FormControl('', Validators.required),
		Username: new FormControl('', Validators.required),
		Password: new FormControl('', Validators.compose([Validators.required, Validators.minLength(6)])),
		Captcha: new FormControl('', Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(4)]))
	});
	logoUrl = require("../../../vendor/images/logo.png");
	msgs: Message[] = [];

	onSubmit() {
		var formValue = this.adminLoginForm.value;
		this.msgs = [];
		this.msgs.push({ severity: 'info', summary: 'Info Message', detail: JSON.stringify(formValue) });
	}

	ngOnInit() {
		this.adminLoginForm.patchValue({ Tenant: "Master" });
	}
}
