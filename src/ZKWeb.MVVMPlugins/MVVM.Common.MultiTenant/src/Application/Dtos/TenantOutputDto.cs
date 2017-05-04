using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Application.Dtos {
	[Description("租户传出信息")]
	public class TenantOutputDto : IOutputDto {
		[Description("租户Id")]
		public Guid Id { get; set; }
		[Description("租户名称")]
		public string Name { get; set; }
		[Description("是否主租户")]
		public bool IsMaster { get; set; }
		[Description("超级管理员名称")]
		public string SuperAdminName { get; set; }
		[Description("创建时间")]
		public string CreateTime { get; set; }
		[Description("更新时间")]
		public string UpdateTime { get; set; }
		[Description("备注")]
		public string Remark { get; set; }
	}
}
