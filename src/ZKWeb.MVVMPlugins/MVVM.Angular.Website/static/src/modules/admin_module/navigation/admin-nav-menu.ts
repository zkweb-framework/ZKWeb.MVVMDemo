import { NavMenuGroup } from './nav-menu-group';
import { NavMenuItem } from './nav-menu-item';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';

// 定义后台导航栏
export const AdminNavMenu: NavMenuGroup[] = [
	{
		name: "System Manage",
		icon: "fa fa-gear",
		items: [
			{
				name: "Tenant Manage",
				icon: "fa fa-done",
				url: ["admin", "tenants"],
				auth: {
					requireMasterTenant: true,
					requireUserType: UserTypes.IAmSuperAdmin,
					requirePrivileges: []
				}
			},
			{
				name: "User Manage",
				icon: "fa fa-user",
				url: ["admin", "users"],
				auth: {
					requireUserType: UserTypes.IAmSuperAdmin,
					requirePrivileges: []
				}
			},
			{
				name: "Role Manage",
				icon: "fa fa-user",
				url: ["admin", "roles"],
				auth: {
					requireUserType: UserTypes.IAmSuperAdmin,
					requirePrivileges: []
				}
			}
		]
	}
];
