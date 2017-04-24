/** 验证要求信息 */
export class AuthRequirement {
	/** 是否要求主租户 */
	requireMasterTenant?: boolean;
	/** 要求的用户类型 */
	requireUserType?: string;
	/** 要求的权限列表 */
	requirePrivileges?: string[];
}
