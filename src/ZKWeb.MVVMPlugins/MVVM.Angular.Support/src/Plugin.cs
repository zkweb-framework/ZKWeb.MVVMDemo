using ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Application;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src {
	/// <summary>
	/// 插件载入时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 插件载入时生成脚本
		/// </summary>
		public Plugin() {
			var generator = ZKWeb.Application.Ioc.Resolve<AngularScriptGenerator>();
			generator.GenerateAll();
		}
	}
}
