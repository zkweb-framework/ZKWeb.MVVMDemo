using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organizationsrc.Domain.Entities.UserTypes
{
    /// <summary>
    /// 用户类型: 管理员
    /// </summary>
    [ExportMany]
    public class AdminUserType : IAmAdmin
    {
        public const string ConstType = "Admin";
        public string Type { get { return ConstType; } }
    }
}
