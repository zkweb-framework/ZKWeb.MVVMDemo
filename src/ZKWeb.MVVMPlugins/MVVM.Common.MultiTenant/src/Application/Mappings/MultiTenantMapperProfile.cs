using AutoMapper;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Application.Mappings
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class MultiTenantMapperProfile : Profile
    {
        public MultiTenantMapperProfile()
        {
            // 租户
            CreateMap<TenantInputDto, Tenant>();
            CreateMap<Tenant, TenantOutputDto>();
        }
    }
}
