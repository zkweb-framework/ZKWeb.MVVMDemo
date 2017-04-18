using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("角色传出信息")]
	public class RoleOutputDto : IOutputDto {
		[Description("角色Id")]
		public Guid Id { get; set; }
		[Description("角色名称")]
		public string Name { get; set; }
		[Description("权限列表")]
		public IList<string> Privileges { get; set; }
		[Description("租户名")]
		public string OwnerTenantName { get; set; }
		[Description("租户Id")]
		public Guid OwnerTenantId { get; set; }
		[Description("创建时间")]
		public string CreateTime { get; set; }
		[Description("更新时间")]
		public string UpdateTime { get; set; }
		[Description("备注")]
		public string Remark { get; set; }
		[Description("已删除")]
		public bool Deleted { get; set; }

		public RoleOutputDto() {
			Privileges = new List<string>();
		}
	}
}
