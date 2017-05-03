using System;
using ZKWeb.Storage;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.ApiExplorer.src.Controllers {
	/// <summary>
	/// 提供Swagger资源
	/// </summary>
	[ExportMany]
	public class SwaggerResourcesController : IController {
		/// <summary>
		/// 返回swagger的额外js
		/// </summary>
		/// <returns></returns>
		[Action("swagger/swagger-site.js")]
		public IActionResult SwaggerSiteJs() {
			var fileStorage = ZKWeb.Application.Ioc.Resolve<IFileStorage>();
			var file = fileStorage.GetResourceFile("static", "swagger.js", "swagger-site.js");
			return new FileEntryResult(file);
		}
	}
}
