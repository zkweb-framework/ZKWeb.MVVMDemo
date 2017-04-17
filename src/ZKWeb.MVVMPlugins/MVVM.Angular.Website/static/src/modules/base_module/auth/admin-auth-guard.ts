﻿import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate } from '@angular/router';

@Injectable()
export class AdminAuthGuard implements CanActivate {
	constructor(private router: Router) { }

	canActivate() {
		this.router.navigate(['admin', 'login']);
		return false;
	}
}
