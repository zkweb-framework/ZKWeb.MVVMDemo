// 用户登录请求
export class UserLoginRequestDto {
	// 租户
	public Tenant: string;
	// 用户名
	public Username: string;
	// 密码
	public Password: string;
	// 验证码
	public Captcha: string;
}
