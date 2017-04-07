using Hangfire;
using ZKWeb.Logging;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ScheduledTasks.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ScheduledTasks {
	/// <summary>
	/// 会话清理器
	/// 每小时删除一次过期的会话
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionCleaner : ScheduledTaskBase<SessionCleaner> {
		/// <summary>
		/// 任务Id
		/// </summary>
		public override string JobId => "SessionCleaner";
		/// <summary>
		/// 执行间隔
		/// </summary>
		public override string CronExpression => Cron.Hourly();

		/// <summary>
		/// 删除过期的会话
		/// </summary>
		public static void Execute() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var count = sessionManager.RemoveExpiredSessions();
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogInfo(string.Format(
				"SessionCleaner executed, {0} sessions removed", count));
		}
	}
}
