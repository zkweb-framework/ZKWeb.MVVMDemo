import { Component } from '@angular/core';
import { SelectItem } from 'primeng/components/common/api';
import { ConfirmationService } from 'primeng/components/common/api';
import { CrudBaseComponent } from '../../base_module/components/crud-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { WebsiteManageService } from '../../generated_module/services/website-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
    selector: 'admin-scheduled-task-log-list',
    templateUrl: '../views/admin-scheduled-task-log-list.html',
    providers: [ConfirmationService]
})
export class AdminScheduledTaskLogListComponent extends CrudBaseComponent {
    isSuccessOptions: SelectItem[];

    constructor(
        appSessionService: AppSessionService,
        appPrivilegeService: AppPrivilegeService,
        appTranslationService: AppTranslationService,
        private websiteManageService: WebsiteManageService) {
        super(appSessionService, appPrivilegeService, appTranslationService);
    }

    ngOnInit() {
        super.ngOnInit();
        this.isSuccessOptions = [
            { label: this.appTranslationService.translate("Please Select"), value: null },
            { label: this.appTranslationService.translate("Yes"), value: true },
            { label: this.appTranslationService.translate("No"), value: false },
        ];
    }

    submitSearch(request: GridSearchRequestDto) {
        return this.websiteManageService.SearchScheduledTaskLogs(request);
    }

    getAddRequirement() { return { requireUserType: UserTypes.IAmAdmin }; }
    getEditRequirement() { return { requireUserType: UserTypes.IAmAdmin }; }
    getRemoveRequirement() { return { requireUserType: UserTypes.IAmAdmin }; }
    add() { }
    edit() { }
    remove() { }
}
