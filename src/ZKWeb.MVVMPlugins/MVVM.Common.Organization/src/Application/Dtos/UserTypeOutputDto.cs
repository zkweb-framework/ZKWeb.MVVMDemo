using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("用户类型的传出信息")]
	public class UserTypeOutputDto : IOutputDto {
		[Description("类型")]
		public string Type { get; set; }
		[Description("描述")]
		public string Description { get; set; }
	}
}
