using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ValidationMessageProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ValidationMessageProviders {
	/// <summary>
	/// 提供验证正则表达式的错误信息
	/// </summary>
	[ExportMany]
	public class RegularExpressionMessageProvider : IValidationMessageProvider<RegularExpressionAttribute> {
		public string FormatErrorMessage(RegularExpressionAttribute attribute, string name) {
			return new T("Format of {0} is incorrect", name);
		}
	}
}
