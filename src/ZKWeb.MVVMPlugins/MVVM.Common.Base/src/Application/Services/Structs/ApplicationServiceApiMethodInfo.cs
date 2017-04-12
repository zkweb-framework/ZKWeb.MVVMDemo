using System;
using System.Collections.Generic;
using ZKWeb.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Structs {
	/// <summary>
	/// 应用服务中的Api函数信息
	/// </summary>
	public class ApplicationServiceApiMethodInfo {
		/// <summary>
		/// 返回类型
		/// </summary>
		public Type ReturnType { get; set; }
		/// <summary>
		/// 函数名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Url地址
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 属性列表
		/// </summary>
		public IEnumerable<Attribute> Attributes { get; set; }
		/// <summary>
		/// 参数列表
		/// </summary>
		public IEnumerable<ApplicationServiceApiParameterInfo> Parameters { get; set; }
		/// <summary>
		/// Api函数主体
		/// </summary>
		public Func<IActionResult> Action { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ApplicationServiceApiMethodInfo(
			Type returnType, string name, string url,
			IEnumerable<Attribute> attributes,
			IEnumerable<ApplicationServiceApiParameterInfo> parameters,
			Func<IActionResult> action) {
			ReturnType = returnType;
			Name = name;
			Url = url;
			Attributes = attributes;
			Parameters = parameters;
			Action = action;
		}
	}
}
