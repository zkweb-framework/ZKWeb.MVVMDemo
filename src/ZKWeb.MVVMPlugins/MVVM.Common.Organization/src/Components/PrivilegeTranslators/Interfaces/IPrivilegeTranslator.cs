namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeTranslators.Interfaces
{
    /// <summary>
    /// 权限名称翻译器
    /// </summary>
    public interface IPrivilegeTranslator
    {
        /// <summary>
        /// 获取权限的本地化名称
        /// </summary>
        /// <param name="privilege">权限</param>
        /// <returns></returns>
        string Translate(string privilege);
    }
}
