using System;
using System.Linq;
using System.Threading;
using ZKWeb.Logging;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Components.ScheduledTasks.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Services {
	/// <summary>
	/// 定时任务管理器
	/// 流程
	/// 从IScheduledTaskProvider获取所有IScheduledTask
	/// 每分钟
	/// - 获取数据库中的最后执行时间
	/// - 调用ShouldExecuteNow函数
	/// - 以事务更新数据库中的最后执行时间
	/// - 更新成功时调用Execute
	/// - 把调用结果写入到日志
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScheduledTaskManager :
		DomainServiceBase<ScheduledTask, string> {
		/// <summary>
		/// 创建执行定时任务的线程
		/// </summary>
		public ScheduledTaskManager() {
			var logManager = Application.Ioc.Resolve<LogManager>();
			var thread = new Thread(() => {
				// 每分钟调查一次是否有需要执行的任务
				while (true) {
					Thread.Sleep(TimeSpan.FromMinutes(1));
					// 获取定时任务
					var tasks = Application.Ioc.ResolveMany<IScheduledTaskProvider>()
						.SelectMany(p => p.GetTasks()).ToList();
					// 枚举并处理定时任务
					foreach (var task in tasks) {
						try {
							HandleTask(task);
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
		/// 处理单个定时任务，返回是否实际执行了
		/// </summary>
		protected virtual bool HandleTaskInner(IScheduledTask executor) {
			var uow = UnitOfWork;
			using (uow.Scope()) {
				// 从数据库获取任务的最后执行时间，判断是否需要立刻执行
				uow.Context.BeginTransaction();
				var task = Get(executor.Key);
				var lastExecuted = task != null ? task.UpdateTime : DateTime.MinValue;
				if (!executor.ShouldExecuteNow(lastExecuted)) {
					return false;
				}
				// 执行前使用事务更新数据库中的执行时间，防止多个进程托管同一个网站时重复执行
				task = task ?? new ScheduledTask() { Id = executor.Key };
				Save(ref task, t => t.UpdateTime = DateTime.UtcNow);
				uow.Context.FinishTransaction();
			}
			// 执行定时任务
			executor.Execute();
			return true;
		}

		/// <summary>
		/// 处理单个定时任务
		/// </summary>
		/// <param name="executor">定时任务执行器</param>
		protected virtual void HandleTask(IScheduledTask executor) {
			// 执行任务并捕捉错误
			bool executed = false;
			bool success = false;
			string error = null;
			try {
				executed = HandleTaskInner(executor);
				success = true;
			} catch (Exception e) {
				error = e.ToString();
			}
			if (!executed && error == null) {
				return;
			}
			// 记录日志
			using (UnitOfWork.Scope()) {
				var logRepository = Application.Ioc.Resolve<IRepository<ScheduledTaskLog, Guid>>();
				var log = new ScheduledTaskLog() {
					Task = Get(executor.Key),
					CreateTime = DateTime.UtcNow,
					Success = success,
					Error = error
				};
				logRepository.Save(ref log);
			}
		}
	}
}
