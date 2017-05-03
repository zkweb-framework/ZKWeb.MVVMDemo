using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("网站信息")]
	public class WebsiteInfoOutputDto : IOutputDto {
		[Description("ZKWeb版本")]
		public string ZKWebVersion { get; set; }
		[Description("ZKWeb完整版本")]
		public string ZKWebFullVersion { get; set; }
		[Description("使用内存")]
		public string MemoryUsage { get; set; }
		[Description("插件列表")]
		public IList<PluginInfoOutputDto> Plugins { get; set; }

		public WebsiteInfoOutputDto() {
			Plugins = new List<PluginInfoOutputDto>();
		}
	}
}
