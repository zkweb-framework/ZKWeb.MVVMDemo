import { Component, OnInit } from '@angular/core';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { WebsiteInfoOutputDto } from '../../generated_module/dtos/website-info-output-dto';
import { AdminToastService } from '../../admin_base_module/services/admin-toast-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';

@Component({
	selector: 'admin-about-me',
	templateUrl: '../views/admin-about-me.html'
})
export class AdminAboutMeComponent implements OnInit {
	tenant: string;
	username: string;
	userType: string;
	roles: string[] = [];
	privileges: string[] = [];

	constructor(
		private appTranslationService: AppTranslationService,
		private adminToastService: AdminToastService,
		private appSessionService: AppSessionService,
		private appPrivilegeService: AppPrivilegeService) {
	}

	ngOnInit() {
		this.appSessionService.getSessionInfo().subscribe(
			s => {
				this.tenant = s.User.OwnerTenantName;
				this.username = s.User.Username;
				this.userType = this.appTranslationService.translate(s.User.Type);
				this.roles = s.User.Roles.map(r => r.Name);
				this.privileges = s.User.Privileges.map(p => this.appPrivilegeService.translatePrivilege(p));
			},
			e => this.adminToastService.showToastMessage("error", e));
	}
}
