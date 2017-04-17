import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
	selector: 'admin-login',
	templateUrl: '../views/admin-login.html',
	styleUrls: ['../styles/admin-login.scss']
})
export class AdminLoginComponent {
	adminLoginForm = new FormGroup({
		Username: new FormControl(),
		Password: new FormControl(),
		Captcha: new FormControl()
	});
	logoUrl = require("../../../vendor/images/logo.png");
}
