using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos
{
    /// <summary>
    /// 用户登录请求
    /// </summary>
    [Description("用户登录请求")]
    public class UserLoginRequestDto : IInputDto
    {
        [Description("租户"), Required]
        public string Tenant { get; set; }
        [Description("用户名"), Required]
        public string Username { get; set; }
        [Description("密码"), Required, StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }
        [Description("验证码"), Required, StringLength(4, MinimumLength = 4)]
        public string Captcha { get; set; }
        [Description("记住登录")]
        public bool? RememberLogin { get; set; }
    }
}
