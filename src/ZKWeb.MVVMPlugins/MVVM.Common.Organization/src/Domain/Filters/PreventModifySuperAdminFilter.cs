using System;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ExtraConfigKeys;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.Server;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Filters {
    /// <summary>
    /// 阻止修改超级管理员的密码, 或删除管理员
    /// </summary>
    [ExportMany, SingletonReuse]
    public class PreventModifySuperAdminFilter : IEntityOperationFilter
    {
        public bool PreventModifySuperAdmin { get; set; }

        public PreventModifySuperAdminFilter()
        {
            var configManager = ZKWeb.Application.Ioc.Resolve<WebsiteConfigManager>();
            var extra = configManager.WebsiteConfig.Extra;
            PreventModifySuperAdmin = extra.GetOrDefault(
                OrganizationExtraConfigKeys.PreventModifySuperAdmin, false);
        }

        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (PreventModifySuperAdmin &&
                entity is User &&
                ((User)(object)entity).OwnerTenant.Name == TenantManager.MasterTenantName &&
                ((User)(object)entity).Username == AdminManager.SuperAdminName)
            {
                throw new NotSupportedException(
                    new T("Modify super admin is disabled, please modify website configuration"));
            }
        }

        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (PreventModifySuperAdmin &&
                entity is User &&
                ((User)(object)entity).OwnerTenant.Name == TenantManager.MasterTenantName &&
                ((User)(object)entity).Username == AdminManager.SuperAdminName) {
                throw new NotSupportedException(
                    new T("Delete super admin is disabled, please modify website configuration"));
            }
        }
    }
}
