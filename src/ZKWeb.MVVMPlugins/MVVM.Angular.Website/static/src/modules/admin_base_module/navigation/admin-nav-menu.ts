import { NavMenuGroup } from './nav-menu-group';
import { NavMenuItem } from './nav-menu-item';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';

/** 定义后台导航栏 */
export const AdminNavMenu: NavMenuGroup[] = [
	{
		name: "Admin Index",
		icon: "fa fa-home",
		items: [],
		url: ["/admin"]
	},
	{
		name: "System Manage",
		icon: "fa fa-gear",
		items: [
			{
				name: "Tenant Manage",
				icon: "fa fa-copy",
				url: ["/admin", "tenants"],
				auth: {
					requireMasterTenant: true,
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [Privileges.Tenant_View]
				}
			},
			{
				name: "User Manage",
				icon: "fa fa-user",
				url: ["/admin", "users"],
				auth: {
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [Privileges.User_View]
				}
			},
			{
				name: "Role Manage",
				icon: "fa fa-legal",
				url: ["/admin", "roles"],
				auth: {
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [Privileges.Role_View]
				}
			}
		]
	},
	{
		name: "System Settings",
		icon: "fa fa-wrench",
		items: [
			{
				name: "Website Settings",
				icon: "fa fa-globe",
				url: ["/admin", "settings", "website_settings"],
				auth: {
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [ /* TODO */]
				}
			},
		]
	}
];
