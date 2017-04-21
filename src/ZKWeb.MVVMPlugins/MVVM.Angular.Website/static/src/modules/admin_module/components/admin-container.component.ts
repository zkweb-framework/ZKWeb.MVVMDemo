import { Component, OnInit } from '@angular/core';
import { NavMenuGroup } from '../navigation/nav-menu-group';
import { AdminNavMenu } from '../navigation/admin-nav-menu';
import { AppSessionService } from '../../auth_module/services/app-session-service';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';

@Component({
	selector: 'admin-container',
	templateUrl: '../views/admin-container.html',
	styleUrls: ['../styles/admin-container.scss']
})
export class AdminContainerComponent implements OnInit {
	logoUrl = require("../../../vendor/images/logo.png");
	activeMenuId: string;
	dropdownVisible: boolean = false;
	mobileMenuActive: boolean = false;
	navMenuGroups: NavMenuGroup[] = [];

	constructor(
		private appSessionService: AppSessionService,
		private appPrivilegeService: AppPrivilegeService) { }

	ngOnInit() {
		// 根据当前用户显示导航栏菜单，过滤无权限的菜单项
		this.navMenuGroups = [];
		this.appSessionService.getSessionInfo().subscribe(sessionInfo => {
			var newMenuGroups = [];
			AdminNavMenu.forEach(group => {
				// 检查分组权限
				if (group.auth != null &&
					!this.appPrivilegeService.isAuthorized(sessionInfo.User, group.auth)) {
					return;
				}
				// 构建新的菜单项列表
				var newGroup: NavMenuGroup = {
					name: group.name,
					icon: group.icon,
					items: [],
					url: group.url,
					auth: group.auth
				};
				(group.items || []).forEach(item => {
					// 检查菜单项权限
					if (item.auth != null &&
						!this.appPrivilegeService.isAuthorized(sessionInfo.User, item.auth)) {
						return;
					}
					newGroup.items.push(item);
				});
				// 如果分组有自己的url，或者分组有一个以上的菜单项则添加到新的列表中
				if (newGroup.url || newGroup.items.length > 0) {
					newMenuGroups.push(newGroup);
				}
			});
			this.navMenuGroups = newMenuGroups;
		});
	}

	toggleMenu(e) {
		// 切换手机版菜单的显示
		this.mobileMenuActive = !this.mobileMenuActive;
		e.preventDefault();
	}
}
