using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("插件信息")]
	public class PluginInfoOutputDto : IOutputDto {
		[Description("目录名称")]
		public string DirectoryName { get; set; }
		[Description("插件名称")]
		public string Name { get; set; }
		[Description("版本")]
		public string Version { get; set; }
		[Description("完整版本")]
		public string FullVersion { get; set; }
		[Description("描述")]
		public string Description { get; set; }
	}
}
