using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 租户管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("租户管理服务")]
	public class TenantManageService : ApplicationServiceBase {
		private TenantManager _tenantManager;

		public TenantManageService(TenantManager tenantManager) {
			_tenantManager = tenantManager;
		}

		[Description("搜索租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:View")]
		public GridSearchResponseDto Search(GridSearchRequestDto request) {
			return request.BuildResponse<Tenant, Guid>()
				.FilterKeywordWith(t => t.Name)
				.FilterKeywordWith(t => t.Remark)
				.ToResponse<TenantOutputDto>();
		}

		[Description("编辑租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Edit")]
		public ActionResponseDto Edit(TenantInputDto dto) {
			var tenant = _tenantManager.Get(dto.Id) ?? new Tenant();
			Mapper.Map(dto, tenant);
			_tenantManager.Save(ref tenant);
			return ActionResponseDto.CreateSuccess("Saved Successfully");
		}

		[Description("删除租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Remove")]
		public ActionResponseDto Remove(Guid id) {
			if (_tenantManager.Count(x => x.Id == id && x.IsMaster) > 0) {
				throw new BadRequestException("You can't delete master tenant");
			}
			_tenantManager.Delete(id);
			return ActionResponseDto.CreateSuccess("Deleted Successfully");
		}

		[Description("获取所有租户")]
		[CheckPrivilege(true, typeof(IAmAdmin))]
		public IList<TenantOutputDto> GetAllTenants() {
			var tenants = _tenantManager.GetMany();
			return tenants.Select(t => Mapper.Map<TenantOutputDto>(t)).ToList();
		}
	}
}
