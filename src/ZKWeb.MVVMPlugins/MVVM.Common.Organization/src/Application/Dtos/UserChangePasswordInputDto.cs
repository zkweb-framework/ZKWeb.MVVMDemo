using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("修改密码传入信息")]
	public class UserChangePasswordInputDto : IInputDto {
		[Description("原密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
		public string OldPassword { get; set; }
		[Description("新密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
		public string NewPassword { get; set; }
		[Description("确认信密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
		public string ConfirmNewPassword { get; set; }
	}
}
