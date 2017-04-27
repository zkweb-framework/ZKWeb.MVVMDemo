using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos {
	[Description("用户传入信息")]
	public class UserInputDto : IInputDto {
		[Description("用户Id")]
		public Guid Id { get; set; }
		[Description("用户类型"), Required]
		public string Type { get; set; }
		[Description("用户名"), Required]
		public string Username { get; set; }
		[Description("密码")]
		public string Password { get; set; }
		[Description("租户Id")]
		public Guid OwnerTenantId { get; set; }
		[Description("备注")]
		public string Remark { get; set; }
		[Description("角色Id列表")]
		public IList<Guid> RoleIds { get; set; }

		public UserInputDto() {
			RoleIds = new List<Guid>();
		}
	}
}
