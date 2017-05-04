using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Services;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 初始化定时任务管理器
			ZKWeb.Application.Ioc.Resolve<ScheduledTaskManager>();
		}
	}
}
