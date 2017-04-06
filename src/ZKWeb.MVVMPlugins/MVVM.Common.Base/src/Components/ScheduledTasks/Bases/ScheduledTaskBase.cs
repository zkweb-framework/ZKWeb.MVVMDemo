using Hangfire;
using System.Threading.Tasks;
using ZKWeb.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ScheduledTasks.Bases {
	/// <summary>
	/// 计划任务的基础类
	/// </summary>
	public abstract class ScheduledTaskBase : IWebsiteStartHandler {
		/// <summary>
		/// 任务Id
		/// </summary>
		public abstract string JobId { get; }
		/// <summary>
		/// 间隔表达式
		/// </summary>
		public abstract string CronExpression { get; }
		/// <summary>
		/// 执行任务的函数
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// 等待Hangfire配置完成后添加任务
		/// </summary>
		public void OnWebsiteStart() {
			Task.Factory.StartNew(async () => {
				while (true) {
					await Task.Delay(100);
					if (JobStorage.Current != null) {
						RecurringJob.AddOrUpdate(JobId, () => Execute(), CronExpression);
						break;
					}
				}
			});
		}
	}
}
