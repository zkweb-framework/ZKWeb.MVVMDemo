using System;
using System.Collections.Generic;
using System.Threading;
using ZKWeb.Logging;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ScheduledTasks.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services {
	/// <summary>
	/// 定时任务管理器
	/// 流程
	/// 收集所有IScheduledTaskExecutor
	/// 每分钟
	/// - 获取数据库中的最后执行时间
	/// - 调用ShouldExecuteNow函数
	/// - 以事务更新数据库中的最后执行时间
	/// - 更新成功时调用Execute
	/// - Execute抛出例外时记录到日志
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScheduledTaskManager :
		DomainServiceBase<ScheduledTask, string> {
		/// <summary>
		/// 定时任务执行器的列表
		/// </summary>
		private Lazy<IEnumerable<IScheduledTaskExecutor>> Executors =
			new Lazy<IEnumerable<IScheduledTaskExecutor>>(() =>
			Application.Ioc.ResolveMany<IScheduledTaskExecutor>());

		/// <summary>
		/// 创建执行定时任务的线程
		/// </summary>
		public ScheduledTaskManager() {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var thread = new Thread(() => {
				// 每分钟调查一次是否有需要执行的任务
				while (true) {
					Thread.Sleep(TimeSpan.FromMinutes(1));
					// 枚举并处理定时任务
					foreach (var executor in Executors.Value) {
						try {
							HandleTask(executor);
						} catch (Exception e) {
							logManager.LogError(e.ToString());
						}
					}
				}
			});
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// 处理单个定时任务
		/// </summary>
		/// <param name="executor">定时任务执行器</param>
		protected virtual void HandleTask(IScheduledTaskExecutor executor) {
			var uow = UnitOfWork;
			using (uow.Scope()) {
				// 从数据库获取任务的最后执行时间，判断是否需要立刻执行
				uow.Context.BeginTransaction();
				var task = Get(executor.Key);
				var lastExecuted = task != null ? task.UpdateTime : DateTime.MinValue;
				if (!executor.ShouldExecuteNow(lastExecuted)) {
					return;
				}
				// 执行前使用事务更新数据库中的执行时间，防止多个进程托管同一个网站时重复执行
				task = task ?? new ScheduledTask() { Id = executor.Key };
				Save(ref task, t => t.UpdateTime = DateTime.UtcNow);
				uow.Context.FinishTransaction();
			}
			// 执行定时任务
			executor.Execute();
		}
	}
}
