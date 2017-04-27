import { Injectable } from '@angular/core';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { AuthRequirement } from '../auth/auth-requirement';
import { UserOutputDto } from '../../generated_module/dtos/user-output-dto';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';

/** 检查权限的结果 */
export class PrivilegeCheckResult {
	/** 是否检查成功 */
	success: boolean;
	/** 检查失败时的s信息 */
	errorMessage: string;
}

/** 权限信息 */
export class PrivilegeInfo {
	/** 权限值 */
	privilege: string;
	/** 权限描述 */
	description: string;
}

/** 检查权限使用的服务 */
@Injectable()
export class AppPrivilegeService {
	constructor(private appTranslationService: AppTranslationService) { }

	/** 翻译权限 */
	translatePrivilege(privilege: string): string {
		var index = privilege.indexOf(':');
		var group = index > 0 ? privilege.substr(0, index) : "Other";
		var name = index > 0 ? privilege.substr(index + 1) : privilege;
		return this.appTranslationService.translate(group) + ":" + this.appTranslationService.translate(name);
	}

	/** 获取所有权限 */
	getAllPrivileges(): PrivilegeInfo[] {
		var result: PrivilegeInfo[] = [];
		for (var key in Privileges) {
			if (Privileges.hasOwnProperty(key)) {
				var privilege = Privileges[key];
				var description = this.translatePrivilege(privilege);
				result.push({ privilege, description });
			}
		}
		return result;
	}

	/** 检查用户是否有满足指定的权限要求 */
	isAuthorized(user: UserOutputDto, auth: AuthRequirement): PrivilegeCheckResult {
		if (user == null) {
			// 未登录时检查失败
			return { success: false, errorMessage: null };
		} else if (auth.requireMasterTenant && !user.OwnerTenantIsMasterTenant) {
			// 要求主租户但是用户不属于主租户时
			var errorMessage = this.appTranslationService
				.translate("Action require user under master tenant");
			return { success: false, errorMessage: errorMessage };
		} else if (auth.requireUserType &&
			user.ImplementedTypes.indexOf(auth.requireUserType) < 0) {
			// 不包含指定用户类型时检查失败
			var errorMessage = this.appTranslationService
				.translate("Action require user to be '{0}'")
				.replace("{0}", this.appTranslationService.translate(auth.requireUserType));
			return { success: false, errorMessage: errorMessage };
		} else if (auth.requirePrivileges &&
			user.ImplementedTypes.indexOf(UserTypes.IAmSuperAdmin) < 0 &&
			auth.requirePrivileges.filter(p => user.Privileges.indexOf(p) < 0).length > 0) {
			// 不包含指定权限时检查失败
			// 如果用户类型是超级管理员则不检查具体权限
			var errorMessage = this.appTranslationService
				.translate("Action require user to be '{0}', and have privileges '{1}'")
				.replace("{0}", this.appTranslationService.translate(auth.requireUserType))
				.replace("{1}", auth.requirePrivileges.map(p => this.translatePrivilege(p)).join());
			return { success: false, errorMessage: errorMessage };
		}
		return { success: true, errorMessage: null };
	}
}
