import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthRequirement } from './auth-requirement';
import { AppConfigService } from '../../base_module/services/app-config-service';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { AppSessionService } from '../services/app-session-service';
import { AppPrivilegeService } from '../services/app-privilege-service';

/** 用于给路由检查权限
	例:
	{
		path: 'example',
		component: ExampleComponent,
		canActivate: [AuthGuard],
		data: {
			auth: {
				requireMasterTenant: false,
				requireUserType: 'ICanUseAdminPanel',
				requirePrivileges: ['Example:Edit']
			}
		}
	} */
@Injectable()
export class AuthGuard implements CanActivate {
	constructor(
		private router: Router,
		private appConfigService: AppConfigService,
		private appTranslationService: AppTranslationService,
		private appSessionService: AppSessionService,
		private appPrivilegeService: AppPrivilegeService) { }

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
		return new Observable(o => {
			this.appSessionService.getSessionInfo().subscribe(
				sessionInfo => {
					// 检查权限
					let routeData = route.data || {};
					let user = sessionInfo.User;
					let auth: AuthRequirement = routeData["auth"];
					let checkResult = this.appPrivilegeService.isAuthorized(user, auth);
					// 检查失败时跳转到登录页面
					if (!checkResult.success) {
						if (checkResult.errorMessage) {
							alert(checkResult.errorMessage);
						}
						this.router.navigate(this.appConfigService.getLoginUrl());
					}
					o.next(checkResult.success);
					o.complete();
				},
				error => {
					alert(error);
					this.router.navigate(this.appConfigService.getLoginUrl());
					o.next(false);
					o.complete();
				});
		});
	}
}
