using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities
{
    /// <summary>
    /// 用户到角色的多对多关联表
    /// </summary>
    [ExportMany]
    public class UserToRole : ManyToManyEntityBase<User, Role, UserToRole>
    {
    }
}
