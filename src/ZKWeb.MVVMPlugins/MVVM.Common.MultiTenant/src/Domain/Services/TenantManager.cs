using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Components.TenantProviders;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services
{
    /// <summary>
    /// 租户管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class TenantManager : DomainServiceBase<Tenant, Guid>
    {
        /// <summary>
        /// 主租户的名称
        /// </summary>
        protected const string MasterTenantName = "Master";
        /// <summary>
        /// 主租户的实例
        /// </summary>
        protected Tenant MasterTenant { get; set; }
        /// <summary>
        /// 生成主租户时使用的锁
        /// </summary>
        protected object MasterTenantLock { get; set; } = new object();

        /// <summary>
        /// 获取当前的租户
        /// 不存在时返回null
        /// </summary>
        /// <returns></returns>
        public virtual Tenant GetTenant()
        {
            var providers = ZKWeb.Application.Ioc.ResolveMany<ITenantProvider>();
            using (var uow = UnitOfWork.Scope())
            {
                foreach (var provider in providers.Reverse())
                {
                    var tennat = provider.GetTenant();
                    if (tennat != null)
                    {
                        return tennat;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 确保主租户的存在
        /// 不存在时创建
        /// 返回原有或创建后的主租户
        /// </summary>
        public virtual Tenant EnsureMasterTenant()
        {
            if (MasterTenant != null)
            {
                return MasterTenant;
            }
            lock (MasterTenantLock)
            {
                if (MasterTenant != null)
                {
                    return MasterTenant;
                }
                using (UnitOfWork.Scope())
                {
                    UnitOfWork.Context.BeginTransaction();
                    var tenant = Repository.Get(t => t.IsMaster == true);
                    if (tenant == null)
                    {
                        tenant = new Tenant() { Name = MasterTenantName, IsMaster = true };
                        Repository.Save(ref tenant);
                    }
                    MasterTenant = tenant;
                    UnitOfWork.Context.FinishTransaction();
                }
            }
            return MasterTenant;
        }
    }
}
