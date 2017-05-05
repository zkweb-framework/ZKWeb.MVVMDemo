using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Repository
{
    /// <summary>
    /// 租户的仓储
    /// </summary>
    [ExportMany]
    public class TenantRepository : RepositoryBase<Tenant, Guid> { }
}
