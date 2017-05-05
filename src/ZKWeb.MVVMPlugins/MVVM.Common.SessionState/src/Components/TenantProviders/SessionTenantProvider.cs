using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Components.TenantProviders;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Components.TenantProviders
{
    /// <summary>
    /// 从会话提供当前租户
    /// </summary>
    [ExportMany]
    public class SessionTenantProvider : ITenantProvider
    {
        /// <summary>
        /// 获取租户
        /// </summary>
        /// <returns></returns>
        public Tenant GetTenant()
        {
            var sessionManager = Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            return session.GetTenant();
        }
    }
}
