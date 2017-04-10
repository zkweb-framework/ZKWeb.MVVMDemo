using System.ComponentModel.DataAnnotations;
using ZKWeb.Cache;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.CacheIsolationPolicies {
	/// <summary>
	/// 按当前登录用户隔离缓存
	/// </summary>
	[ExportMany(ContractKey = "Ident")]
	public class CacheIsolateByIdent : ICacheIsolationPolicy {
		/// <summary>
		/// 获取隔离键
		/// </summary>
		/// <returns></returns>
		public object GetIsolationKey() {
			var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
			return sessionManager.GetSession().ReleatedId;
		}
	}
}
