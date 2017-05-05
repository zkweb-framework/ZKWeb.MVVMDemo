using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.UserTypes
{
    /// <summary>
    /// 匿名用户
    /// </summary>
    [ExportMany]
    public class AnonymouseUserType : IAmAnonymouseUser
    {
        public string Type { get { return null; } }
    }
}
