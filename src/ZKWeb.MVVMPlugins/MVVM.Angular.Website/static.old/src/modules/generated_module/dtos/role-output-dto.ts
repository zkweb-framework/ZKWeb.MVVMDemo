/** 角色传出信息 */
export class RoleOutputDto {
    /** 角色Id */
    public Id: string;
    /** 角色名称 */
    public Name: string;
    /** 权限列表 */
    public Privileges: string[];
    /** 权限名称列表 */
    public PrivilegeNames: string;
    /** 租户名 */
    public OwnerTenantName: string;
    /** 租户Id */
    public OwnerTenantId: string;
    /** 创建时间 */
    public CreateTime: string;
    /** 更新时间 */
    public UpdateTime: string;
    /** 备注 */
    public Remark: string;
    /** 已删除 */
    public Deleted: boolean;
}
