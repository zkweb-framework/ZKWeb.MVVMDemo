using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Application.Dtos
{
    [Description("租户传入信息")]
    public class TenantInputDto : IInputDto
    {
        [Description("租户Id")]
        public Guid Id { get; set; }
        [Description("租户名称")]
        public string Name { get; set; }
        [Description("超级管理员名称"), Required]
        public string SuperAdminName { get; set; }
        [Description("超级管理员密码"), StringLength(int.MaxValue, MinimumLength = 6)]
        public string SuperAdminPassword { get; set; }
        [Description("超级管理员确认密码"), StringLength(int.MaxValue, MinimumLength = 6)]
        public string SuperAdminConfirmPassword { get; set; }
        [Description("备注")]
        public string Remark { get; set; }
    }
}
