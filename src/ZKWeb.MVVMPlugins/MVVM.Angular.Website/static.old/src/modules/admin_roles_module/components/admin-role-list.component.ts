import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/components/common/api';
import { ConfirmationService } from 'primeng/components/common/api';
import { CrudWithDialogBaseComponent } from '../../base_module/components/crud-with-dialog-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { RoleManageService } from '../../generated_module/services/role-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
    selector: 'admin-role-list',
    templateUrl: '../views/admin-role-list.html',
    providers: [ConfirmationService]
})
export class AdminRoleListComponent extends CrudWithDialogBaseComponent {
    privilegeOptions: SelectItem[];

    constructor(
        confirmationService: ConfirmationService,
        appSessionService: AppSessionService,
        appPrivilegeService: AppPrivilegeService,
        appTranslationService: AppTranslationService,
        private roleManageService: RoleManageService) {
        super(confirmationService, appSessionService, appPrivilegeService, appTranslationService);
    }

    ngOnInit() {
        super.ngOnInit();
        this.privilegeOptions = [];
        this.appPrivilegeService.getAllPrivileges(this.isMasterTenant).forEach(info => {
            this.privilegeOptions.push({ label: info.description, value: info.privilege });
        });
        this.editForm.addControl("Id", new FormControl(""));
        this.editForm.addControl("Name", new FormControl("", Validators.required));
        this.editForm.addControl("Privileges", new FormControl([]));
        this.editForm.addControl("Remark", new FormControl(""));
    }

    submitSearch(request: GridSearchRequestDto) {
        return this.roleManageService.Search(request);
    }

    getAddRequirement() {
        return {
            requireMasterRole: true,
            requireUserType: UserTypes.IAmAdmin,
            requirePrivileges: [Privileges.Role_Edit]
        };
    }

    getEditRequirement() {
        return {
            requireUserType: UserTypes.IAmAdmin,
            requirePrivileges: [Privileges.Role_Edit]
        };
    }

    getRemoveRequirement() {
        return {
            requireUserType: UserTypes.IAmAdmin,
            requirePrivileges: [Privileges.Role_Remove]
        };
    }

    submitEdit(obj: any) {
        return this.roleManageService.Edit(obj);
    }

    submitRemove(obj: any) {
        return this.roleManageService.Remove(obj.Id);
    }
}
