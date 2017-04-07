using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.ComponentModel;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMDemo.AspNetCore.Swagger {
	/// <summary>
	/// 为参数或返回类型提供注释
	/// </summary>
	public class ZKWebSwaggerSchemaFilter : ISchemaFilter {
		/// <summary>
		/// 处理Schema
		/// </summary>
		public void Apply(Schema model, SchemaFilterContext context) {
			// 设置模型自身的描述
			model.Description = context.SystemType
				.GetTypeInfo()
				.GetCustomAttribute<DescriptionAttribute>()?.Description ??
				context.SystemType.Name;
			if (model.Properties == null) {
				return;
			}
			// 设置成员的描述
			// Swagger会强制设置所有成员的名称为驼峰命名即使不指定选项，这里矫正回去
			var descriptionMap = context.SystemType.FastGetProperties()
				.Select(p => new {
					name = p.Name,
					description = p.GetCustomAttribute<DescriptionAttribute>()?.Description ?? p.Name
				})
				.ToDictionary(p => p.name.ToLower());
			model.Properties = model.Properties.Select(property => {
				var pair = descriptionMap.GetOrDefault(property.Key.ToLower());
				var key = pair?.name ?? property.Key;
				property.Value.Description = pair?.description ?? key;
				return new { key, value = property.Value };
			}).ToDictionary(p => p.key, p => p.value);
		}
	}
}
