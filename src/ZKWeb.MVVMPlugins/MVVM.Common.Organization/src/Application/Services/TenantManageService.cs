using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Filters;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.UserTypes;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 租户管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("租户管理服务")]
	public class TenantManageService : ApplicationServiceBase {
		private TenantManager _tenantManager;
		private UserManager _userManager;

		public TenantManageService(TenantManager tenantManager, UserManager userManager) {
			_tenantManager = tenantManager;
			_userManager = userManager;
		}

		/// <summary>
		/// 更新Dto中的超级管理员名称
		/// </summary>
		private void UpdateSuperAdminName(IEnumerable<TenantOutputDto> tenants) {
			var tenantIds = tenants.Select(t => t.Id).ToList();
			var tenantToSuperAdmins = _userManager.GetSuperAdminsFromTenants(tenantIds);
			foreach (var tenant in tenants) {
				tenant.SuperAdminName = tenantToSuperAdmins.GetOrDefault(tenant.Id)?.Username;
			}
		}

		[Description("搜索租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:View")]
		public GridSearchResponseDto Search(GridSearchRequestDto request) {
			var response = request.BuildResponse<Tenant, Guid>()
				.FilterKeywordWith(t => t.Name)
				.FilterKeywordWith(t => t.Remark)
				.ToResponse<TenantOutputDto>();
			UpdateSuperAdminName(response.Result.OfType<TenantOutputDto>());
			return response;
		}

		[Description("编辑租户")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Tenant:Edit")]
		[UnitOfWork(IsTransactional = true)]
		public ActionResponseDto Edit(TenantInputDto dto) {
			var tenant = _tenantManager.Get(dto.Id) ?? new Tenant();
			Mapper.Map(dto, tenant);
			bool isNewTenant = tenant.Id == Guid.Empty;
			// 保存租户
			_tenantManager.Save(ref tenant);
			// 保存超级管理员
			using (UnitOfWork.DisableFilter(typeof(OwnerTenantFilter))) {
				var superAdmin = _userManager.Get(u =>
					u.Username == dto.SuperAdminName &&
					u.OwnerTenantId == tenant.Id) ?? new User();
				superAdmin.Type = SuperAdminUserType.ConstType;
				superAdmin.Username = dto.SuperAdminName;
				superAdmin.OwnerTenant = tenant;
				if (dto.SuperAdminPassword != dto.SuperAdminConfirmPassword) {
					throw new BadRequestException(new T("Confirm password not matched with password"));
				} else if (!string.IsNullOrEmpty(dto.SuperAdminPassword)) {
					superAdmin.SetPassword(dto.SuperAdminPassword);
				} else if (superAdmin.Id == Guid.Empty) {
					throw new BadRequestException(new T("Please provider a password for new user"));
				}
				_userManager.Save(ref superAdmin);
			}
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
	}
}
