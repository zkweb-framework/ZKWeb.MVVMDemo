import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NavMenuGroup } from '../navigation/nav-menu-group';
import { AdminNavMenu } from '../navigation/admin-nav-menu';
import { AppConfigService } from '../../base_module/services/app-config-service';
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
	defaultAvatarUrl: string = require("../../../vendor/images/default-avatar.jpg");
	avatarUrl: string = this.defaultAvatarUrl;
	username: string;

	constructor(
		private router: Router,
		private appConfigService: AppConfigService,
		private appSessionService: AppSessionService,
		private appPrivilegeService: AppPrivilegeService) { }

	ngOnInit() {
		// 更新当前登录用户的信息
		this.navMenuGroups = [];
		this.avatarUrl = this.defaultAvatarUrl;
		this.username = null;
		this.appSessionService.getSessionInfo().subscribe(sessionInfo => {
			var user = sessionInfo.User;
			// 根据当前用户显示导航栏菜单，过滤无权限的菜单项
			var newMenuGroups = [];
			AdminNavMenu.forEach(group => {
				// 检查分组权限
				if (group.auth != null &&
					!this.appPrivilegeService.isAuthorized(user, group.auth)) {
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
						!this.appPrivilegeService.isAuthorized(user, item.auth)) {
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
			// 更新当前用户的头像地址
			if (user.AvatarImageBase64) {
				this.avatarUrl = "data:image/png;base64," + user.AvatarImageBase64;
			} else {
				this.avatarUrl = this.defaultAvatarUrl;
			}
			// 更新当前用户的用户名
			this.username = user.Username;
		});
	}

	/** 切换手机版菜单的显示 */
	toggleMenu(e) {
		this.mobileMenuActive = !this.mobileMenuActive;
		e.preventDefault();
	}

	/** 清理缓存 */
	clearCache(e) {
		alert("TODO");
		e.preventDefault();
	}

	/** 退出登录 */
	logout(e) {
		this.appConfigService.setSessionId("");
		this.router.navigate(['/']);
	}
}
