import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AppConfigService } from '../../base_module/services/app-config-service';
import { AppSessionService } from '../services/app-session-service';

@Injectable()
export class AuthGuard implements CanActivate {
	constructor(
		private router: Router,
		private appConfigService: AppConfigService,
		private appSessionService: AppSessionService) { }

	canActivate(): Observable<boolean> {
		return new Observable(o => {
			setTimeout(() => {
				this.router.navigate(this.appConfigService.getLoginUrl());
				o.next(false);
				o.complete();
			}, 8000);
		});
		/* return new Observable(o => {
			this.appSessionService.getSessionInfo().subscribe(
				sessionInfo => {
					alert(sessionInfo);
					this.router.navigate(this.appConfigService.getLoginUrl());
					o.next(false);
					o.complete();
				},
				error => {
					alert(error);
					this.router.navigate(this.appConfigService.getLoginUrl());
					o.next(false);
					o.complete();
				});
		}); */
	}
}
