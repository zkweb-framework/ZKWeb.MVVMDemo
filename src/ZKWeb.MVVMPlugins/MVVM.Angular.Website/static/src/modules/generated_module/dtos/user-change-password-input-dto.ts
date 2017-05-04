/** 修改密码传入信息 */
export class UserChangePasswordInputDto {
	/** 原密码 */
	public OldPassword: string;
	/** 新密码 */
	public NewPassword: string;
	/** 确认信密码 */
	public ConfirmNewPassword: string;
}
