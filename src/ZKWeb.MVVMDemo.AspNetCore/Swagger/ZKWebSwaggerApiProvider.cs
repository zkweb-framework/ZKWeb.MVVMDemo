using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ZKWeb.MVVMDemo.AspNetCore.Swagger {
	/// <summary>
	/// Swagger Api列表提供器
	/// </summary>
	public class ZKWebSwaggerApiProvider : IApiDescriptionGroupCollectionProvider {
		/// <summary>
		/// 提供Api列表
		/// </summary>
		public ApiDescriptionGroupCollection ApiDescriptionGroups {
			get {
				var provider = Application.Ioc.Resolve<IApiDescriptionGroupCollectionProvider>();
				return provider.ApiDescriptionGroups;
			}
		}
	}
}
