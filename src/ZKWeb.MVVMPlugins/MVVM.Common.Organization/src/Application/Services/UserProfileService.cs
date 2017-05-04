using System.ComponentModel;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 用户资料服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("用户资料服务")]
	public class UserProfileService : ApplicationServiceBase {
		private UserManager _userManager;
		private SessionManager _sessionManager;

		public UserProfileService(
			UserManager userManager,
			SessionManager sessionManager) {
			_userManager = userManager;
			_sessionManager = sessionManager;
		}

		[Description("修改密码")]
		[CheckPrivilege(typeof(IAmUser))]
		public ActionResponseDto ChangePassword(UserChangePasswordInputDto dto) {
			if (dto.NewPassword != dto.ConfirmNewPassword)
				throw new BadRequestException("Confirm password not matched with password");
			var userId = _sessionManager.GetSession().UserId.Value;
			_userManager.ChangePassword(userId, dto.OldPassword, dto.NewPassword);
			return ActionResponseDto.CreateSuccess("Change Password Successfully");
		}

		[Description("上传头像")]
		[CheckPrivilege(typeof(IAmUser))]
		public ActionResponseDto UploadAvatar(UserUploadAvatarInputDto dto) {
			var userId = _sessionManager.GetSession().UserId.Value;
			_userManager.SaveAvatar(userId, dto.Avatar.OpenReadStream());
			return ActionResponseDto.CreateSuccess("Upload Avatar Successfully");
		}
	}
}
