using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMDemo.AspNetCore.Swagger {
	/// <summary>
	/// 为函数提供注释
	/// </summary>
	public class ZKWebSwaggerOperationFilter : IOperationFilter {
		/// <summary>
		/// 处理Operation
		/// </summary>
		public void Apply(Operation operation, OperationFilterContext context) {
			// 设置函数自身的描述
			var attributes = context.ApiDescription.Properties["Attributes"] as IList<Attribute>;
			operation.Description = attributes?
				.OfType<DescriptionAttribute>()
				.FirstOrDefault()?.Description ??
				context.ApiDescription.ActionDescriptor.DisplayName;
			// 设置参数的描述
			if (operation.Parameters == null) {
				return;
			}
			var parametersAttributeMap = context.ApiDescription.Properties["ParameterAttributeMap"]
				as IDictionary<string, IList<Attribute>> ??
				new Dictionary<string, IList<Attribute>>();
			foreach (var parameter in operation.Parameters) {
				var parameterAttributes = parametersAttributeMap.GetOrDefault(parameter.Name);
				parameter.Description = parameterAttributes?
					.OfType<DescriptionAttribute>()
					.FirstOrDefault()?.Description ?? parameter.Name;
			}
		}
	}
}
