using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("网站设置")]
	public class WebsiteSettingsDto : IInputDto, IOutputDto {
		[Description("网站名称")]
		public string WebsiteName { get; set; }
	}
}
