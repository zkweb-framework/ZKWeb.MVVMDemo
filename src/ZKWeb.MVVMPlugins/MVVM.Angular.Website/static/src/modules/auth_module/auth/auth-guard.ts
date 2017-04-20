import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AppConfigService } from '../../base_module/services/app-config-service';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { AppSessionService } from '../services/app-session-service';
import { UserTypes } from '../../generated_module/privileges/user-types';

// 用于给路由检查权限
// 例:
// {
//	path: 'example',
//	component: ExampleComponent,
//	canActivate: [AuthGuard],
//	data: { requireUserType: 'ICanUseAdminPanel', requirePrivileges: [ 'Example:Edit' ] }
// }
@Injectable()
export class AuthGuard implements CanActivate {
	constructor(
		private router: Router,
		private appConfigService: AppConfigService,
		private appTranslationService: AppTranslationService,
		private appSessionService: AppSessionService) { }

	translatePrivilege(privilege: string): string {
		var index = privilege.indexOf(':');
		var group = index > 0 ? privilege.substr(0, index) : "Other";
		var name = index > 0 ? privilege.substr(index + 1) : privilege;
		return this.appTranslationService.translate(group) + ":" + this.appTranslationService.translate(name);
	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
		return new Observable(o => {
			this.appSessionService.getSessionInfo().subscribe(
				sessionInfo => {
					// 检查用户类型和权限
					var routeData = route.data || {};
					var requireUserType: string = routeData["requireUserType"];
					var requirePrivileges: string[] = routeData["requirePrivileges"];
					var checkSuccess = false;
					var errorMessage = null;
					if (sessionInfo.User == null) {
						// 未登录时检查失败
					} else if (requireUserType &&
						sessionInfo.User.ImplementedTypes.indexOf(requireUserType) < 0) {
						// 不包含指定用户类型时检查失败
						errorMessage = this.appTranslationService
							.translate("Action require user to be '{0}'")
							.replace("{0}", this.appTranslationService.translate(requireUserType));
					} else if (requirePrivileges &&
						sessionInfo.User.ImplementedTypes.indexOf(UserTypes.IAmSuperAdmin) < 0 &&
						requirePrivileges.filter(p =>
							sessionInfo.User.Privileges.indexOf(p) < 0).length > 0) {
						// 不包含指定权限时检查失败
						// 如果用户类型是超级管理员则不检查具体权限
						errorMessage = this.appTranslationService
							.translate("Action require user to be '{0}', and have privileges '{1}'")
							.replace("{0}", this.appTranslationService.translate(requireUserType))
							.replace("{1}", requirePrivileges.map(p => this.translatePrivilege(p)).join());
					} else {
						checkSuccess = true;
					}
					// 检查失败时跳转到登录页面
					if (!checkSuccess) {
						errorMessage && alert(errorMessage);
						this.router.navigate(this.appConfigService.getLoginUrl());
					}
					o.next(checkSuccess);
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
