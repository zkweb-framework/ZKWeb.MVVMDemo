using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 租户管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("租户管理服务")]
	public class TenantManageService : ApplicationServiceBase {
		[Description("搜索租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:View")]
		public GridSearchResponseDto Search(GridSearchRequestDto request) {
			return request.BuildResponse<Tenant, Guid>()
				.FilterKeywordWith(t => t.Name)
				.FilterKeywordWith(t => t.Remark)
				.ToResponse();
		}

		[Description("编辑租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Edit")]
		public ActionResponseDto Edit(object dto) {
			throw new NotImplementedException();
		}

		[Description("删除租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Remove")]
		public ActionResponseDto Remove(Guid id) {
			throw new NotImplementedException();
		}
	}
}
