import { RoleOutputDto } from './role-output-dto';

// 用户传出信息
export class UserOutputDto {
	// 用户Id
	public Id: any;
	// 用户类型
	public Type: string;
	// 用户名
	public Name: string;
	// 租户名
	public OwnerTenantName: string;
	// 租户Id
	public OwnerTenantId: any;
	// 创建时间
	public CreateTime: string;
	// 更新时间
	public UpdateTime: string;
	// 已删除
	public Deleted: boolean;
	// 角色列表
	public Roles: RoleOutputDto[];
}
