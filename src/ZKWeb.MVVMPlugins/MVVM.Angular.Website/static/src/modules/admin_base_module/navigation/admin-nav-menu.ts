import { NavMenuGroup } from './nav-menu-group';
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
					requirePrivileges: [Privileges.Settings_WebsiteSettings]
				}
			},
		]
	},
	{
		name: "Scheduled Tasks",
		icon: "fa fa-tasks",
		items: [
			{
				name: "Scheduled Tasks",
				icon: "fa fa-tasks",
				url: ["/admin", "scheduled_tasks"],
				auth: {
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [Privileges.ScheduledTask_View]
				}
			},
			{
				name: "Scheduled Tasks Log",
				icon: "fa fa-tasks",
				url: ["/admin", "scheduled_tasks", "log"],
				auth: {
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [Privileges.ScheduledTask_View]
				}
			},
		]
	},
	{
		name: "Example Datas",
		icon: "fa fa-database",
		items: [
			{
				name: "Example Datas",
				icon: "fa fa-database",
				url: ["/admin", "example_datas"],
				auth: {
					requireUserType: UserTypes.IAmAdmin,
					requirePrivileges: [Privileges.ExampleData_View]
				}
			},
		]
	}
];
