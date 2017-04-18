using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeTranslators.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// 权限管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PrivilegeManager : DomainServiceBase {
		/// <summary>
		/// 获取网站中注册的所有权限，并且去除重复项
		/// </summary>
		/// <returns></returns>
		public virtual List<string> GetPrivileges() {
			var providers = ZKWeb.Application.Ioc.ResolveMany<IPrivilegesProvider>();
			var privileges = providers.SelectMany(p => p.GetPrivileges()).Distinct().ToList();
			return privileges;
		}

		/// <summary>
		/// 检查当前的用户类型是否继承了指定的类型，且是否拥有指定的权限
		/// 如果用户类型不匹配或无指定的权限时抛出403错误	
		/// </summary>
		/// <param name="userType">用户类型，例如typeof(IAmAdmin)</param>
		/// <param name="privileges">要求的权限列表</param>
		public virtual void Check(Type userType, params string[] privileges) {
			var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var userTypeMatched = HasUserType(user, userType);
			var context = HttpManager.CurrentContext;
			if (userTypeMatched && HasPrivileges(user, privileges)) {
				// 检查通过
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
				throw new ForbiddenException(
					new T("Action require {0}, and {1} privileges",
					new T(userType.Name),
					string.Join(",", privileges.Select(p => translator.Translate(p)))));
			} else {
				// 用户类型不符合，或未登录
				throw new ForbiddenException(
					new T("Action require {0}", new T(userType.Name)));
			}
		}

		/// <summary>
		/// 判断用户是否拥有指定的用户类型
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="userType">用户类型的接口或基础类</param>
		/// <returns></returns>
		public virtual bool HasUserType(User user, Type userType) {
			return userType.GetTypeInfo().IsAssignableFrom(user.GetUserType().GetType());
		}

		/// <summary>
		/// 判断用户是否拥有指定的权限
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="privileges">权限列表</param>
		/// <returns></returns>
		public virtual bool HasPrivileges(User user, params string[] privileges) {
			var userType = user?.GetUserType();
			if (userType is IAmSuperAdmin) {
				// 超级管理员拥有所有权限
				return true;
			}
			if (privileges != null && privileges.Length > 0) {
				var containsPrivileges = new HashSet<string>(
					user.Roles.SelectMany(r => r.To.GetPrivileges()));
				foreach (var privilege in privileges) {
					if (!containsPrivileges.Contains(privilege)) {
						// 未包含指定的所有权限
						return false;
					}
				}
			}
			// 检查通过
			return true;
		}
	}
}
