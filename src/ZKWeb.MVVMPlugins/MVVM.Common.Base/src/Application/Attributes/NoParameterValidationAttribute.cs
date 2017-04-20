using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes {
	/// <summary>
	/// 标记此函数不对传入的数据传输对象进行验证
	/// 仅在应用服务中的函数上有效
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class NoParameterValidationAttribute : Attribute {
	}
}
