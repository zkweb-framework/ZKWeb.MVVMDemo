using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("修改头像传入信息")]
	public class UserUploadAvatarInputDto : IInputDto {
		[Description("头像文件"), Required, CheckFile]
		public IHttpPostedFile Avatar { get; set; }
	}
}
