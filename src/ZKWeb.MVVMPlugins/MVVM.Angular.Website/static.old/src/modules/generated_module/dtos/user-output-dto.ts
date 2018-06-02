import { RoleOutputDto } from './role-output-dto';

/** 用户传出信息 */
export class UserOutputDto {
    /** 用户Id */
    public Id: string;
    /** 用户类型 */
    public Type: string;
    /** 用户名 */
    public Username: string;
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
    /** 角色Id列表 */
    public RoleIds: string[];
    /** 角色列表 */
    public Roles: RoleOutputDto[];
    /** 租户名 */
    public OwnerTenantName: string;
    /** 租户是主租户 */
    public OwnerTenantIsMasterTenant: boolean;
    /** 头像图片的Base64 */
    public AvatarImageBase64: string;
    /** 实现的用户类型列表 */
    public ImplementedTypes: string[];
    /** 权限列表 */
    public Privileges: string[];
}
