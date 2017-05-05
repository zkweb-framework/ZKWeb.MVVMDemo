import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Message, MenuItem } from 'primeng/primeng';
import { NavMenuGroup } from '../navigation/nav-menu-group';
import { AdminNavMenu } from '../navigation/admin-nav-menu';
import { AppConfigService } from '../../base_module/services/app-config-service';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { WebsiteManageService } from '../../generated_module/services/website-manage-service';
import { AdminToastService } from '../services/admin-toast-service';

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
	switchLanguageItems: MenuItem[] = [];
	switchTimezoneItems: MenuItem[] = [];

	constructor(
		private router: Router,
		private appConfigService: AppConfigService,
		private appTranslationService: AppTranslationService,
		private appSessionService: AppSessionService,
		private appPrivilegeService: AppPrivilegeService,
		private websiteManageService: WebsiteManageService,
		private adminToastService: AdminToastService) { }

	ngOnInit() {
		// 更新当前登录用户的信息
		this.navMenuGroups = [];
		this.avatarUrl = this.defaultAvatarUrl;
		this.username = null;
		this.appSessionService.getSessionInfo().subscribe(sessionInfo => {
			let user = sessionInfo.User;
			// 根据当前用户显示导航栏菜单，过滤无权限的菜单项
			let newMenuGroups = [];
			AdminNavMenu.forEach(group => {
				// 检查分组权限
				if (group.auth != null &&
					!this.appPrivilegeService.isAuthorized(user, group.auth).success) {
					return;
				}
				// 构建新的菜单项列表
				let newGroup: NavMenuGroup = {
					name: group.name,
					icon: group.icon,
					items: [],
					url: group.url,
					auth: group.auth
				};
				(group.items || []).forEach(item => {
					// 检查菜单项权限
					console.log("check", item.auth);
					if (item.auth != null &&
						!this.appPrivilegeService.isAuthorized(user, item.auth).success) {
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
				this.avatarUrl = "data:image/jpeg;base64," + user.AvatarImageBase64;
			} else {
				this.avatarUrl = this.defaultAvatarUrl;
			}
			// 更新当前用户的用户名
			this.username = user.Username;
		});
		// 更新切换语言的菜单
		this.switchLanguageItems = [
			{
				label: this.appTranslationService.translate("zh-CN"),
				icon: "fa-language",
				command: () => this.switchLanguage("zh-CN")
			},
			{
				label: this.appTranslationService.translate("en-US"),
				icon: "fa-language",
				command: () => this.switchLanguage("en-US")
			}
		];
		// 更新切换时区的菜单
		this.switchTimezoneItems = [
			{
				label: this.appTranslationService.translate("Asia/Shanghai"),
				icon: "fa-clock-o",
				command: () => this.switchTimezone("Asia/Shanghai")
			},
			{
				label: this.appTranslationService.translate("America/New_York"),
				icon: "fa-clock-o",
				command: () => this.switchTimezone("America/New_York")
			}
		];
	}

	/** 切换手机版菜单的显示 */
	toggleMenu(e) {
		this.mobileMenuActive = !this.mobileMenuActive;
		e.preventDefault();
	}

	/** 获取悬浮信息列表 */
	getToastMessages(): Message[] {
		return this.adminToastService.getToastMessages();
	}

	/** 清理缓存 */
	clearCache(e) {
		this.websiteManageService.ClearCache().subscribe(
			s => {
				this.adminToastService.showToastMessage("info", s.Message);
				this.dropdownVisible = false;
			},
			ee => this.adminToastService.showToastMessage("error", e));
		e.preventDefault();
	}

	/** 退出登录 */
	logout(e) {
		this.appConfigService.setSessionId("");
		this.router.navigate(['/']);
		e.preventDefault();
	}

	/** 切换语言 */
	switchLanguage(language: string) {
		this.appConfigService.setLanguage(language);
		location.href = location.href;
	}

	/** 切换时区 */
	switchTimezone(timezone: string) {
		this.appConfigService.setTimezone(timezone);
		location.href = location.href;
	}
}
