using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Swagger {
	/// <summary>
	/// 根据应用服务获取Api列表
	/// </summary>
	[ExportMany]
	public class SwaggerApiProvider : IApiDescriptionGroupCollectionProvider {
		public ApiDescriptionGroupCollection ApiDescriptionGroups {
			get {
				var groups = new List<ApiDescriptionGroup>();
				var collection = new ApiDescriptionGroupCollection(groups, 1);
				return collection;
			}
		}
	}
}
