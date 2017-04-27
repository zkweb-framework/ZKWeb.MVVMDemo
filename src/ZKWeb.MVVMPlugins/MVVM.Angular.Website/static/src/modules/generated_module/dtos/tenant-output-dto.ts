/** 租户传出信息 */
export class TenantOutputDto {
	/** 租户Id */
	public Id: string;
	/** 租户名称 */
	public Name: string;
	/** 是否主租户 */
	public IsMaster: boolean;
	/** 创建时间 */
	public CreateTime: string;
	/** 更新时间 */
	public UpdateTime: string;
	/** 备注 */
	public Remark: string;
}
