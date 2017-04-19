using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	/// <summary>
	/// 用户登录请求
	/// </summary>
	[Description("用户登录请求")]
	public class UserLoginRequestDto : IInputDto {
		[Description("租户")]
		public string Tenant { get; set; }
		[Description("用户名")]
		public string Username { get; set; }
		[Description("密码")]
		public string Password { get; set; }
		[Description("验证码")]
		public string Captcha { get; set; }
		[Description("记住登录")]
		public bool? RememberLogin { get; set; }
	}
}
