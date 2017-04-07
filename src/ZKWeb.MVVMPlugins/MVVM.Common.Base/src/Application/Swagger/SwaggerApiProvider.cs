using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using System.ComponentModel;
using System.FastReflection;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Swagger {
	/// <summary>
	/// 根据应用服务获取Api列表
	/// </summary>
	[ExportMany]
	public class SwaggerApiProvider : IApiDescriptionGroupCollectionProvider {
		/// <summary>
		/// 提供防止Swagger出错的反射信息
		/// </summary>
		public class DummyClass {
			public object DummyMethod(string dummyArg) { return null; }
		}

		/// <summary>
		/// 获取Api列表
		/// </summary>
		public ApiDescriptionGroupCollection ApiDescriptionGroups {
			get {
				// 获取假的TypeInfo和MethodInfo
				// 传给Swagger防止它在ApiDescriptionExtensions.ActionAttributes中出错
				var dummyType = typeof(DummyClass).GetTypeInfo();
				var dummyMethod = typeof(DummyClass).FastGetMethod(nameof(DummyClass.DummyMethod));
				var dummyArg = dummyMethod.GetParameters()[0];
				// 枚举添加应用服务中的Api函数
				var groups = new List<ApiDescriptionGroup>();
				var services = ZKWeb.Application.Ioc.ResolveMany<IApplicationService>();
				foreach (var service in services) {
					// 分组名称
					var typeInfo = service.GetType().GetTypeInfo();
					var typeDescription = typeInfo.GetAttribute<DescriptionAttribute>();
					var groupName = typeDescription?.Description ?? typeInfo.Name;
					// 函数列表
					var items = new List<ApiDescription>();
					foreach (var methodInfo in service.GetApiMethods()) {
						// descriptor的类型必须是ControllerActionDescriptor
						// 否则会在ApiDescriptionExtensions.ControllerActionDescriptor出错
						var description = new ApiDescription();
						var descriptor = new ControllerActionDescriptor();
						descriptor.ControllerName = groupName;
						descriptor.ActionName = methodInfo.Name;
						descriptor.DisplayName = methodInfo.Attributes
							.OfType<DescriptionAttribute>().FirstOrDefault()?.Description ??
							methodInfo.Name;
						descriptor.MethodInfo = methodInfo.MethodInfo ?? dummyMethod;
						descriptor.ControllerTypeInfo = typeInfo;
						descriptor.RouteValues = new Dictionary<string, string>();
						descriptor.AttributeRouteInfo = new AttributeRouteInfo();
						descriptor.AttributeRouteInfo.Name = methodInfo.Name;
						descriptor.AttributeRouteInfo.Template = "";
						descriptor.ActionConstraints = new List<IActionConstraintMetadata>();
						descriptor.Parameters = new List<ParameterDescriptor>();
						descriptor.BoundProperties = new List<ParameterDescriptor>();
						foreach (var parameter in methodInfo.Parameters) {
							var parameterDescriptor = new ControllerParameterDescriptor();
							parameterDescriptor.Name = parameter.Name;
							parameterDescriptor.ParameterType = parameter.Type;
							parameterDescriptor.BindingInfo = new BindingInfo();
							parameterDescriptor.ParameterInfo = parameter.ParameterInfo ?? dummyArg;
							descriptor.Parameters.Add(parameterDescriptor);
							descriptor.BoundProperties.Add(parameterDescriptor);
						}
						descriptor.FilterDescriptors = new List<FilterDescriptor>();
						descriptor.Properties = new Dictionary<object, object>();
						description.ActionDescriptor = descriptor;
						description.GroupName = groupName;
						description.HttpMethod = "POST";
						description.RelativePath = methodInfo.Url.Substring(1);
						items.Add(description);
					}
					var group = new ApiDescriptionGroup(groupName, items);
					groups.Add(group);
				}
				var collection = new ApiDescriptionGroupCollection(groups, 1);
				return collection;
			}
		}
	}
}
