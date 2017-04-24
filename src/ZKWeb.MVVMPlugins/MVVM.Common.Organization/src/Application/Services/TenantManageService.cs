using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 租户管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("租户管理服务")]
	public class TenantManageService : ApplicationServiceBase {
		[Description("搜索租户")]
		public GridSearchResponseDto Search(GridSearchRequestDto dto) {
			var result = new GridSearchResponseDto();
			result.TotalCount = 100;
			result.Result.Add(Mapper.Map<TenantOutputDto>(new Tenant() { Name = "a" }));
			result.Result.Add(Mapper.Map<TenantOutputDto>(new Tenant() { Name = "b" }));
			result.Result.Add(Mapper.Map<TenantOutputDto>(new Tenant() { Name = "c" }));
			return result;
		}
	}
}
