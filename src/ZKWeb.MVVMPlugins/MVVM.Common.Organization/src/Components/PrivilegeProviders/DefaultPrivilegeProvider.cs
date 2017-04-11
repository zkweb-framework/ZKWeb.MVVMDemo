using System;
using System.Collections.Generic;
using ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeProviders {
	/// <summary>
	/// 默认的权限提供器
	/// </summary>
	[ExportMany]
	public class DefaultPrivilegeProvider : IPrivilegesProvider {
		/// <summary>
		/// 查找应用服务中的权限并返回
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetPrivileges() {
			// TODO
			throw new NotImplementedException();
		}
	}
}
