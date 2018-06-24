/** 角色传入信息 */
export class RoleInputDto {
    /** 角色Id */
    public Id: string;
    /** 角色名称 */
    public Name: string;
    /** 备注 */
    public Remark: string;
    /** 权限列表 */
    public Privileges: string[];
}
