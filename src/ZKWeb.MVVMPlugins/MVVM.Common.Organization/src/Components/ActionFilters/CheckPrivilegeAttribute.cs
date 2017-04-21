using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Structs;
using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters {
	/// <summary>
	/// 用于检查权限的属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class CheckPrivilegeAttribute : ActionFilterAttribute {
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
		/// Http方法
		/// 如果等于空则所有Http方法都会检查
		/// </summary>
		public string HttpMethod { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CheckPrivilegeAttribute(Type requiredUserType, params string[] privileges) {
			RequireUserType = requiredUserType;
			RequirePrivileges = privileges;
			HttpMethod = null;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public CheckPrivilegeAttribute(
			bool requireMasterTenant,
			Type requiredUserType, params string[] privileges) {
			RequireMasterTenant = requireMasterTenant;
			RequireUserType = requiredUserType;
			RequirePrivileges = privileges;
			HttpMethod = null;
		}

		/// <summary>
		/// 执行前检查权限
		/// </summary>
		public override Func<IActionResult> Filter(Func<IActionResult> action) {
			return () => {
				if (string.IsNullOrEmpty(HttpMethod) ||
					HttpMethod.Equals(
						HttpManager.CurrentContext.Request.Method,
						StringComparison.OrdinalIgnoreCase)) {
					var requirement = new AuthRequirement(
						RequireMasterTenant,
						RequireUserType,
						RequirePrivileges);
					var privilegeManager = ZKWeb.Application.Ioc.Resolve<PrivilegeManager>();
					privilegeManager.Check(requirement);
				}
				return action();
			};
		}
	}
}
