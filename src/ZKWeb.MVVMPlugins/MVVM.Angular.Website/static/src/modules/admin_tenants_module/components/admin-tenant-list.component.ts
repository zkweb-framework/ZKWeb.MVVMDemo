import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SelectItem } from 'primeng/primeng';
import { CrudWithDialogBaseComponent } from '../../base_module/components/crud-with-dialog-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { TenantManageService } from '../../generated_module/services/tenant-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';
import { AuthRequirement } from '../../auth_module/auth/auth-requirement';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
	selector: 'admin-tenant-list',
	templateUrl: '../views/admin-tenant-list.html'
})
export class AdminTenantListComponent extends CrudWithDialogBaseComponent {
	isMasterOptions: SelectItem[];

	constructor(
		router: Router,
		appSessionService: AppSessionService,
		appPrivilegeService: AppPrivilegeService,
		appTranslationService: AppTranslationService,
		private tenantManageService: TenantManageService) {
		super(router, appSessionService, appPrivilegeService, appTranslationService);
	}

	ngOnInit() {
		super.ngOnInit();
		this.isMasterOptions = [
			{ label: this.appTranslationService.translate("Please Select"), value: null },
			{ label: this.appTranslationService.translate("Yes"), value: true },
			{ label: this.appTranslationService.translate("No"), value: false },
		];
	}

	submitSearch(request: GridSearchRequestDto) {
		return this.tenantManageService.Search(request);
	}

	getAddRequirement() {
		return {
			requireMasterTenant: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.Tenant_Edit]
		};
	}

	getEditRequirement() {
		return {
			requireMasterTenant: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.Tenant_Edit]
		};
	}

	getRemoveRequirement() {
		return {
			requireMasterTenant: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.Tenant_Remove]
		};
	}
}
