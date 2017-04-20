using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Extensions;
using ZKWeb.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ActionParameterProviders {
	/// <summary>
	/// 获取参数后自动进行检查
	/// </summary>
	public class ValidatedActionParameterProvider : IActionParameterProvider {
		private IActionParameterProvider _originalProvider;

		public ValidatedActionParameterProvider(IActionParameterProvider originalProvider) {
			_originalProvider = originalProvider;
		}

		public T GetParameter<T>(string name, MethodInfo method, ParameterInfo parameterInfo) {
			var result = _originalProvider.GetParameter<T>(name, method, parameterInfo);
			// 如果结果是IInputDto并且函数未标记不验证的属性则执行验证
			if (result is IInputDto &&
				method.GetCustomAttribute<NoParameterValidationAttribute>() == null) {
				((IInputDto)result).Validate();
			}
			return result;
		}
	}
}
