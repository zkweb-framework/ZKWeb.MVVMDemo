using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ValidationMessageProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ValidationMessageProviders
{
    /// <summary>
    /// 提供验证字符串长度的错误信息
    /// </summary>
    [ExportMany]
    public class StringLengthMessageProvider : IValidationMessageProvider<StringLengthAttribute>
    {
        public string FormatErrorMessage(StringLengthAttribute attribute, string name)
        {
            if (attribute.MinimumLength == 0)
            {
                // 只指定了最大长度
                return new T("Length of {0} must not greater than {1}", name, attribute.MaximumLength);
            }
            else if (attribute.MaximumLength == int.MaxValue)
            {
                // 只指定了最小长度
                return new T("Length of {0} must not less than {1}", name, attribute.MinimumLength);
            }
            return new T("Length of {0} must between {1} and {2}",
                name, attribute.MinimumLength, attribute.MaximumLength);
        }
    }
}
