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
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Swagger {
	/// <summary>
	/// 根据应用服务获取Api列表
	/// </summary>
	[ExportMany]
	public class SwaggerApiProvider : IApiDescriptionGroupCollectionProvider {
		/// <summary>
		/// 提供防止Swagger出错的反射信息
		/// </summary>
		private class DummyClass {
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
				// 获取ModelMetadata的生成器
				var serviceProvider = ZKWeb.Application.Ioc.Resolve<IServiceProvider>();
				var modelMetadataProvider = (IModelMetadataProvider)serviceProvider
					.GetService(typeof(IModelMetadataProvider));
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
						var descriptor = new ControllerActionDescriptor() {
							ControllerName = groupName,
							ActionName = methodInfo.Name,
							DisplayName = methodInfo.Name,
							MethodInfo = dummyMethod,
							ControllerTypeInfo = dummyType,
							RouteValues = new Dictionary<string, string>(),
							AttributeRouteInfo = new AttributeRouteInfo() {
								Name = methodInfo.Name,
								Template = ""
							},
							ActionConstraints = new List<IActionConstraintMetadata>(),
							Parameters = new List<ParameterDescriptor>(),
							BoundProperties = new List<ParameterDescriptor>(),
							FilterDescriptors = new List<FilterDescriptor>(),
							Properties = new Dictionary<object, object>()
						};
						var description = new ApiDescription() {
							ActionDescriptor = descriptor,
							GroupName = groupName,
							HttpMethod = "POST",
							RelativePath = methodInfo.Url.Substring(1),
						};
						// 枚举参数列表
						var parameters = methodInfo.Parameters.ToList();
						var parameterAttributeMap = new Dictionary<string, IList<Attribute>>();
						foreach (var parameter in parameters) {
							var parameterDescriptor = new ControllerParameterDescriptor() {
								Name = parameter.Name,
								ParameterType = parameter.Type,
								BindingInfo = new BindingInfo(),
								ParameterInfo = dummyArg
							};
							descriptor.Parameters.Add(parameterDescriptor);
							var apiParameterDescription = new ApiParameterDescription() {
								ModelMetadata = (
									modelMetadataProvider.GetMetadataForType(parameter.Type)),
								Name = parameter.Name,
								RouteInfo = new ApiParameterRouteInfo(),
								Type = parameter.Type
							};
							// 只有1个参数并且该参数是mscorlib之外的class时默认来源是json
							// 否则默认来源是form
							var parameterType = parameter.Type.GetTypeInfo();
							if (parameters.Count == 1 &&
								parameterType.IsClass &&
								parameterType.Assembly != typeof(string).GetTypeInfo().Assembly) {
								apiParameterDescription.Source = BindingSource.Body;
							} else {
								apiParameterDescription.Source = BindingSource.Form;
							}
							description.ParameterDescriptions.Add(apiParameterDescription);
							// 设置参数的属性列表
							parameterAttributeMap[parameter.Name] = parameter.Attributes.ToList();
						}
						description.SupportedRequestFormats.Add(new ApiRequestFormat() {
							MediaType = "application/json"
						});
						description.SupportedResponseTypes.Add(new ApiResponseType() {
							ApiResponseFormats = new List<ApiResponseFormat>() {
								new ApiResponseFormat() { MediaType = "application/json" }
							},
							ModelMetadata = modelMetadataProvider
								.GetMetadataForType(methodInfo.ReturnType),
							Type = methodInfo.ReturnType,
							StatusCode = 200
						});
						description.Properties["ParameterAttributeMap"] = parameterAttributeMap;
						description.Properties["Attributes"] = methodInfo.Attributes.ToList();
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
