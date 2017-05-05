import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/primeng';
import { CrudWithDialogBaseComponent } from '../../base_module/components/crud-with-dialog-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { ExampleDataManageService } from '../../generated_module/services/example-data-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
	selector: 'admin-example-data-list',
	templateUrl: '../views/admin-example-data-list.html',
	providers: [ConfirmationService]
})
export class AdminExampleDataListComponent extends CrudWithDialogBaseComponent {
	constructor(
		confirmationService: ConfirmationService,
		appSessionService: AppSessionService,
		appPrivilegeService: AppPrivilegeService,
		appTranslationService: AppTranslationService,
		private exampleDataManageService: ExampleDataManageService) {
		super(confirmationService, appSessionService, appPrivilegeService, appTranslationService);
	}

	ngOnInit() {
		super.ngOnInit();
		this.editForm.addControl("Id", new FormControl(""));
		this.editForm.addControl("Name", new FormControl("", Validators.required));
		this.editForm.addControl("Description", new FormControl([]));
	}

	submitSearch(request: GridSearchRequestDto) {
		return this.exampleDataManageService.Search(request);
	}

	getAddRequirement() {
		return {
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.ExampleData_Edit]
		};
	}

	getEditRequirement() {
		return {
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.ExampleData_Edit]
		};
	}

	getRemoveRequirement() {
		return {
			requireMasterExampleData: true,
			requireUserType: UserTypes.IAmAdmin,
			requirePrivileges: [Privileges.ExampleData_Remove]
		};
	}

	submitEdit(obj: any) {
		return this.exampleDataManageService.Edit(obj);
	}

	submitRemove(obj: any) {
		return this.exampleDataManageService.Remove(obj.Id);
	}
}
