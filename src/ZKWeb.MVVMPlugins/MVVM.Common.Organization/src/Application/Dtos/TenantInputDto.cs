using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("租户传入信息")]
	public class TenantInputDto : IInputDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Remark { get; set; }
	}
}
