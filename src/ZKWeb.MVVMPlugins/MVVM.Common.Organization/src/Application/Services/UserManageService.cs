using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("用户管理服务")]
    public class UserManageService : ApplicationServiceBase
    {
        private UserManager _userManager;
        private TenantManager _teantManager;

        public UserManageService(
            UserManager userManager,
            TenantManager tenantManager)
        {
            _userManager = userManager;
            _teantManager = tenantManager;
        }

        [Description("搜索用户")]
        [CheckPrivilege(typeof(IAmAdmin), "User:View")]
        public GridSearchResponseDto Search(GridSearchRequestDto request)
        {
            return request.BuildResponse<User, Guid>()
                .FilterKeywordWith(t => t.Username)
                .FilterKeywordWith(t => t.Remark)
                .FilterColumnWith(
                    nameof(UserOutputDto.Roles),
                    (c, q) =>
                    {
                        var roleIds = c.Value.ConvertOrDefault<IList<Guid>>();
                        if (roleIds != null)
                        {
                            return q.Where(u => u.Roles.Any(r => roleIds.Contains(r.To.Id)));
                        }
                        return q;
                    })
                .ToResponse<UserOutputDto>();
        }

        [Description("编辑用户")]
        [CheckPrivilege(typeof(IAmAdmin), "User:Edit")]
        public ActionResponseDto Edit(UserInputDto dto)
        {
            var user = _userManager.Get(dto.Id) ?? new User();
            Mapper.Map(dto, user);
            // 设置用户密码
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.SetPassword(dto.Password);
            }
            else if (user.Id == Guid.Empty)
            {
                throw new BadRequestException("Please provider a password for new user");
            }
            // 保存用户
            _userManager.Save(ref user);
            // 设置角色列表
            if (dto.RoleIds != null)
            {
                _userManager.AssignRoles(user, dto.RoleIds);
            }
            return ActionResponseDto.CreateSuccess("Saved Successfully");
        }

        [Description("删除用户")]
        [CheckPrivilege(typeof(IAmAdmin), "User:Remove")]
        public ActionResponseDto Remove(Guid id)
        {
            _userManager.Delete(id);
            return ActionResponseDto.CreateSuccess("Deleted Successfully");
        }

        [Description("获取所有用户类型")]
        [CheckPrivilege(typeof(IAmAdmin))]
        public IList<UserTypeOutputDto> GetAllUserTypes()
        {
            var userTypes = _userManager.GetAllUserTypes();
            return userTypes.Select(t => new UserTypeOutputDto()
            {
                Type = t.Type,
                Description = new T(t.Type)
            }).ToList();
        }
    }
}
