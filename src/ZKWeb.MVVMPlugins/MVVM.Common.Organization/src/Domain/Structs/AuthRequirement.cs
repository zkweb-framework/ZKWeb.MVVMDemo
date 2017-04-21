using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Structs {
	/// <summary>
	/// 要求的权限信息
	/// </summary>
	public class AuthRequirement {
		/// <summary>
		/// 是否要求主租户
		/// </summary>
		public bool RequireMasterTenant { get; set; }
		/// <summary>
		/// 要求的用户类型
		/// </summary>
		public Type RequireUserType { get; set; }
		/// <summary>
		/// 要求的权限列表
		/// </summary>
		public string[] RequirePrivileges { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AuthRequirement() {
			RequirePrivileges = new string[0];
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public AuthRequirement(bool requireMasterTenant, Type requireUserType, string[] requirePrivileges) {
			RequireMasterTenant = requireMasterTenant;
			RequireUserType = requireUserType;
			RequirePrivileges = requirePrivileges;
		}
	}
}
