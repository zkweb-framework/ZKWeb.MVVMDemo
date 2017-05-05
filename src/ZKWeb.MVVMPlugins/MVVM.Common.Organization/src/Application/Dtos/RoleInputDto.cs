using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos
{
    [Description("角色传入信息")]
    public class RoleInputDto : IInputDto
    {
        [Description("角色Id")]
        public Guid Id { get; set; }
        [Description("角色名称"), Required]
        public string Name { get; set; }
        [Description("备注")]
        public string Remark { get; set; }
        [Description("权限列表")]
        public IList<string> Privileges { get; set; }
    }
}
