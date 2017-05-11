import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/components/common/api';
import { CrudBaseComponent } from '../../base_module/components/crud-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { WebsiteManageService } from '../../generated_module/services/website-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
    selector: 'admin-scheduled-task-list',
    templateUrl: '../views/admin-scheduled-task-list.html',
    providers: [ConfirmationService]
})
export class AdminScheduledTaskListComponent extends CrudBaseComponent {
    constructor(
        appSessionService: AppSessionService,
        appPrivilegeService: AppPrivilegeService,
        appTranslationService: AppTranslationService,
        private websiteManageService: WebsiteManageService) {
        super(appSessionService, appPrivilegeService, appTranslationService);
    }

    submitSearch(request: GridSearchRequestDto) {
        return this.websiteManageService.SearchScheduledTasks(request);
    }

    getAddRequirement() { return { requireUserType: UserTypes.IAmAdmin }; }
    getEditRequirement() { return { requireUserType: UserTypes.IAmAdmin }; }
    getRemoveRequirement() { return { requireUserType: UserTypes.IAmAdmin }; }
    add() { }
    edit() { }
    remove() { }
}
