import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class AdminAuthService {
	private authed = false;

	constructor(private router: Router) {

	}

	isAuthed() {
		return this.authed;
	}

	login(username: string, password: string) {
		return new Promise(resolve => {
			setTimeout(() => {
				this.authed = true;
				resolve();
			}, 1000);
		});
	}
}
