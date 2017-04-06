using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMDemo.Plugins.MVVM.Angular.Support.src.Application {
	/// <summary>
	/// 负责处理AngularJS的服务脚本
	/// </summary>
	[ExportMany]
	public class AngularScriptGenerator : IWebsiteStartHandler {
		/// <summary>
		/// 网站启动时生成脚本
		/// </summary>
		public void OnWebsiteStart() {
			// TODO
		}
	}
}
