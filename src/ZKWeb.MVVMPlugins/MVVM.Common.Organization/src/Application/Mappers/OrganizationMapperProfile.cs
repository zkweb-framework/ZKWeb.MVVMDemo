using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeTranslators.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.Storage;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Mappers
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class OrganizationMapperProfile : Profile
    {
        public OrganizationMapperProfile(UserManager userManager)
        {
            // 用户
            CreateMap<UserInputDto, User>()
                .ForMember(d => d.OwnerTenantId, m => m.Ignore()); // 租户Id为了安全原因需要手动设置
            CreateMap<User, UserOutputDto>()
                .ForMember(d => d.OwnerTenantName, m => m.ResolveUsing(u => u.OwnerTenant?.Name))
                .ForMember(d => d.RoleIds, m => m.ResolveUsing(u => u.Roles.Select(r => r.To.Id).ToList()))
                .ForMember(d => d.Roles, m => m.ResolveUsing(u => u.Roles.Select(r => r.To).ToList()))
                .ForMember(
                    d => d.OwnerTenantIsMasterTenant,
                    m => m.ResolveUsing(u => u.OwnerTenant?.IsMaster ?? false))
                .ForMember(
                    d => d.ImplementedTypes,
                    m => m.ResolveUsing(u => u.GetImplementedUserTypes().Select(t => t.Name).ToList()))
                .ForMember(d => d.Privileges, m => m.ResolveUsing(u => u.GetPrivileges().ToList()))
                .ForMember(
                    d => d.AvatarImageBase64,
                    m => m.ResolveUsing(u =>
                    {
                        var file = userManager.GetAvatarStorageFile(u.Id);
                        return file.Exists ? Convert.ToBase64String(file.ReadAllBytes()) : null;
                    }));

            // 角色
            CreateMap<RoleInputDto, Role>()
                .ForMember(
                    d => d.PrivilegesJson,
                    m => m.ResolveUsing(u => JsonConvert.SerializeObject(u.Privileges)));
            CreateMap<Role, RoleOutputDto>()
                .ForMember(d => d.Privileges, m => m.ResolveUsing(r => r.GetPrivileges()))
                .ForMember(
                    d => d.PrivilegeNames,
                    m => m.ResolveUsing(r =>
                    {
                        var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
                        return string.Join(",", r.GetPrivileges().Select(p => translator.Translate(p)));
                    }))
                .ForMember(d => d.OwnerTenantName, m => m.ResolveUsing(r => r.OwnerTenant?.Name));

            // 网站设置
            CreateMap<WebsiteSettingsDto, WebsiteSettings>();
            CreateMap<WebsiteSettings, WebsiteSettingsDto>();
        }
    }
}
