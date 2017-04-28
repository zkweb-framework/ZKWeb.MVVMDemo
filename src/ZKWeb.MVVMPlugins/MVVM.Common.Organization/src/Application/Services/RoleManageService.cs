using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 角色管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("角色管理服务")]
	public class RoleManageService : ApplicationServiceBase {
		private RoleManager _roleManager;

		public RoleManageService(RoleManager roleManager) {
			_roleManager = roleManager;
		}

		[Description("搜索角色")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Role:View")]
		public GridSearchResponseDto Search(GridSearchRequestDto request) {
			return request.BuildResponse<Role, Guid>()
				.FilterKeywordWith(t => t.Name)
				.FilterKeywordWith(t => t.Remark)
				.ToResponse<RoleOutputDto>();
		}

		[Description("编辑角色")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Role:Edit")]
		public ActionResponseDto Edit(RoleInputDto dto) {
			var role = _roleManager.Get(dto.Id) ?? new Role();
			Mapper.Map(dto, role);
			_roleManager.Save(ref role);
			return ActionResponseDto.CreateSuccess("Saved Successfully");
		}

		[Description("删除角色")]
		[CheckPrivilege(true, typeof(IAmAdmin), "Role:Remove")]
		public ActionResponseDto Remove(Guid id) {
			_roleManager.Delete(id);
			return ActionResponseDto.CreateSuccess("Deleted Successfully");
		}

		[Description("获取所有角色")]
		public IList<RoleOutputDto> GetAllRoles() {
			var roles = _roleManager.GetMany();
			return roles.Select(r => Mapper.Map<RoleOutputDto>(r)).ToList();
		}
	}
}
