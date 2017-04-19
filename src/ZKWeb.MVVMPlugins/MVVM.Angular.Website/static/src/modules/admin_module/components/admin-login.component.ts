import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Message } from 'primeng/primeng';

import { UserLoginService } from '../../generated_module/services/user-login-service';

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

	constructor(
		private router: Router,
		private userLoginService: UserLoginService) { }

	onSubmit() {
		this.userLoginService.LoginAdmin(this.adminLoginForm.value).subscribe(
			result => { this.router.navigate(['']) },
			error => { this.msgs = [{ severity: 'error', detail: error }]; });
	}

	ngOnInit() {
		this.adminLoginForm.patchValue({ Tenant: "Master" });
	}
}
