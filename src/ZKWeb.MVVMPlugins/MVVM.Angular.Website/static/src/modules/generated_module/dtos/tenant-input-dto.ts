/** 租户传入信息 */
export class TenantInputDto {
	/** 租户Id */
	public Id: string;
	/** 租户名称 */
	public Name: string;
	/** 超级管理员名称 */
	public SuperAdminName: string;
	/** 超级管理员密码 */
	public SuperAdminPassword: string;
	/** 超级管理员确认密码 */
	public SuperAdminConfirmPassword: string;
	/** 备注 */
	public Remark: string;
}
