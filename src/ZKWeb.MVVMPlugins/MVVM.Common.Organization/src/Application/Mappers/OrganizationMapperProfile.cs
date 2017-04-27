using AutoMapper;
using Newtonsoft.Json;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeTranslators.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Mappers {
	/// <summary>
	/// AutoMapper的配置
	/// </summary>
	[ExportMany]
	public class OrganizationMapperProfile : Profile {
		public OrganizationMapperProfile() {
			// 租户
			CreateMap<TenantInputDto, Tenant>();
			CreateMap<Tenant, TenantOutputDto>();

			// 用户
			CreateMap<User, UserOutputDto>()
				.ForMember(d => d.OwnerTenantName, m => m.ResolveUsing(u => u.OwnerTenant?.Name))
				.ForMember(
					d => d.OwnerTenantIsMasterTenant,
					m => m.ResolveUsing(u => u.OwnerTenant?.IsMaster ?? false))
				.ForMember(
					d => d.ImplementedTypes,
					m => m.ResolveUsing(u => u.GetImplementedUserTypes().Select(t => t.Name).ToList()))
				.ForMember(d => d.Privileges, m => m.ResolveUsing(u => u.GetPrivileges().ToList()));

			// 角色
			CreateMap<RoleInputDto, Role>()
				.ForMember(
					d => d.PrivilegesJson,
					m => m.ResolveUsing(u => JsonConvert.SerializeObject(u.Privileges)));
			CreateMap<Role, RoleOutputDto>()
				.ForMember(d => d.Privileges, m => m.ResolveUsing(r => r.GetPrivileges()))
				.ForMember(
					d => d.PrivilegeNames,
					m => m.ResolveUsing(r => {
						var translator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
						return string.Join(",", r.GetPrivileges().Select(p => translator.Translate(p)));
					}))
				.ForMember(d => d.OwnerTenantName, m => m.ResolveUsing(r => r.OwnerTenant?.Name));
		}
	}
}
