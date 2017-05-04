import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/primeng';
import { CrudWithDialogBaseComponent } from '../../base_module/components/crud-with-dialog-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { TenantManageService } from '../../generated_module/services/tenant-manage-service';
import { RoleManageService } from '../../generated_module/services/role-manage-service';
import { UserManageService } from '../../generated_module/services/user-manage-service';
import { UserOutputDto } from '../../generated_module/dtos/user-output-dto';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';
import { AuthRequirement } from '../../auth_module/auth/auth-requirement';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
	selector: 'admin-user-list',
	templateUrl: '../views/admin-user-list.html',
	providers: [ConfirmationService]
})
export class AdminUserListComponent extends CrudWithDialogBaseComponent {
	roleOptions: SelectItem[];
	userTypeOptions: SelectItem[];
	defaultAvatarUrl: string = require("../../../vendor/images/default-avatar.jpg");

	constructor(
		confirmationService: ConfirmationService,
		appSessionService: AppSessionService,
		appPrivilegeService: AppPrivilegeService,
		appTranslationService: AppTranslationService,
		private tenantManageService: TenantManageService,
		private roleManageService: RoleManageService,
		private userManageService: UserManageService) {
		super(confirmationService, appSessionService, appPrivilegeService, appTranslationService);
	}

	ngOnInit() {
		super.ngOnInit();
		this.roleOptions = [];
		this.userTypeOptions = [];
		this.roleManageService.GetAllRoles().subscribe(roles => {
			roles.forEach(role => {
				this.roleOptions.push({ label: role.Name, value: role.Id });
			});
		});
		this.userTypeOptions.push({ label: this.appTranslationService.translate("Please Select"), value: null });
		this.userManageService.GetAllUserTypes().subscribe(userTypes => {
			userTypes.forEach(userType => {
				this.userTypeOptions.push({ label: userType.Description, value: userType.Type });
			});
		});
		this.editForm.addControl("Id", new FormControl(""));
		this.editForm.addControl("Type", new FormControl("", Validators.required));
		this.editForm.addControl("Username", new FormControl("", Validators.required));
		this.editForm.addControl("Password", new FormControl("", Validators.minLength(6)));
		this.editForm.addControl("ConfirmPassword", new FormControl("", Validators.minLength(6)));
		this.editForm.addControl("RoleIds", new FormControl([]));
		this.editForm.addControl("Remark", new FormControl(""));
	}

	getRoleNames(row: UserOutputDto) {
		return row.Roles.map(r => r.Name).join(',');
	}

	submitSearch(request: GridSearchRequestDto) {
		return this.userManageService.Search(request);
	}

	getAddRequirement() {
		return {
			requireMasterUser: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.User_Edit]
		};
	}

	getEditRequirement() {
		return {
			requireMasterUser: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.User_Edit]
		};
	}

	getRemoveRequirement() {
		return {
			requireMasterUser: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.User_Remove]
		};
	}

	submitEdit(obj: any) {
		return this.userManageService.Edit(obj);
	}

	submitRemove(obj: any) {
		return this.userManageService.Remove(obj.Id);
	}

	getAvatarUrl(row: UserOutputDto) {
		if (row.AvatarImageBase64) {
			return "data:image/jpeg;base64," + row.AvatarImageBase64;
		} else {
			return this.defaultAvatarUrl;
		}
	}
}
