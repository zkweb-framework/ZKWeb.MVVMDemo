using System.ComponentModel.DataAnnotations;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ValidationMessageProviders.Interfaces
{
    /// <summary>
    /// 验证信息的提供器
    /// </summary>
    /// <typeparam name="T">验证属性类型</typeparam>
    public interface IValidationMessageProvider<T>
        where T : ValidationAttribute
    {
        /// <summary>
        /// 格式化错误消息
        /// </summary>
        /// <param name="attribute">属性对象</param>
        /// <param name="name">对象名称</param>
        /// <returns></returns>
        string FormatErrorMessage(T attribute, string name);
    }
}
