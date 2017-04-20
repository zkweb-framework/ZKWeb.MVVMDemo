using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("用户传出信息")]
	public class UserOutputDto : IOutputDto {
		[Description("用户Id")]
		public Guid Id { get; set; }
		[Description("用户类型")]
		public string Type { get; set; }
		[Description("实现的用户类型列表")]
		public IList<string> ImplementedTypes { get; set; }
		[Description("用户名")]
		public string Name { get; set; }
		[Description("租户名")]
		public string OwnerTenantName { get; set; }
		[Description("租户Id")]
		public Guid OwnerTenantId { get; set; }
		[Description("创建时间")]
		public string CreateTime { get; set; }
		[Description("更新时间")]
		public string UpdateTime { get; set; }
		[Description("已删除")]
		public bool Deleted { get; set; }
		[Description("角色列表")]
		public IList<RoleOutputDto> Roles { get; set; }

		public UserOutputDto() {
			ImplementedTypes = new List<string>();
			Roles = new List<RoleOutputDto>();
		}
	}
}
