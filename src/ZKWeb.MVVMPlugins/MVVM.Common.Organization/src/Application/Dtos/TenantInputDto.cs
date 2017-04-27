using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("租户传入信息")]
	public class TenantInputDto : IInputDto {
		[Description("租户Id")]
		public Guid Id { get; set; }
		[Description("租户名称")]
		public string Name { get; set; }
		[Description("备注")]
		public string Remark { get; set; }
	}
}
