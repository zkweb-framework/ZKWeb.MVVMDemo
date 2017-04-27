/** 用户传入信息 */
export class UserInputDto {
	/** 用户Id */
	public Id: string;
	/** 用户类型 */
	public Type: string;
	/** 用户名 */
	public Username: string;
	/** 密码 */
	public Password: string;
	/** 租户Id */
	public OwnerTenantId: string;
	/** 备注 */
	public Remark: string;
	/** 角色Id列表 */
	public RoleIds: string[];
}
