import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/components/common/api';
import { ConfirmationService } from 'primeng/components/common/api';
import { CrudWithDialogBaseComponent } from '../../base_module/components/crud-with-dialog-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { TenantManageService } from '../../generated_module/services/tenant-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
    selector: 'admin-tenant-list',
    templateUrl: '../views/admin-tenant-list.html',
    providers: [ConfirmationService]
})
export class AdminTenantListComponent extends CrudWithDialogBaseComponent {
    isMasterOptions: SelectItem[];

    constructor(
        confirmationService: ConfirmationService,
        appSessionService: AppSessionService,
        appPrivilegeService: AppPrivilegeService,
        appTranslationService: AppTranslationService,
        private tenantManageService: TenantManageService) {
        super(confirmationService, appSessionService, appPrivilegeService, appTranslationService);
    }

    ngOnInit() {
        super.ngOnInit();
        this.isMasterOptions = [
            { label: this.appTranslationService.translate("Please Select"), value: null },
            { label: this.appTranslationService.translate("Yes"), value: true },
            { label: this.appTranslationService.translate("No"), value: false },
        ];
        this.editForm.addControl("Id", new FormControl(""));
        this.editForm.addControl("Name", new FormControl("", Validators.required));
        this.editForm.addControl("SuperAdminName", new FormControl("", Validators.required));
        this.editForm.addControl("SuperAdminPassword", new FormControl("", Validators.minLength(6)));
        this.editForm.addControl("SuperAdminConfirmPassword", new FormControl("", Validators.minLength(6)));
        this.editForm.addControl("Remark", new FormControl(""));
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

    submitEdit(obj: any) {
        return this.tenantManageService.Edit(obj);
    }

    submitRemove(obj: any) {
        return this.tenantManageService.Remove(obj.Id);
    }
}
