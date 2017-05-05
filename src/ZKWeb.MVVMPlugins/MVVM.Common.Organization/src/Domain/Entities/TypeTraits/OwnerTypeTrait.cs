using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.TypeTraits
{
    /// <summary>
    /// 判断类型是否有所属的用户
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public static class OwnerTypeTrait<TEntity>
    {
        /// <summary>
        /// 判断结果
        /// </summary>
        public readonly static bool HaveOwner =
            typeof(IHaveOwner).GetTypeInfo().IsAssignableFrom(typeof(TEntity));
    }
}
