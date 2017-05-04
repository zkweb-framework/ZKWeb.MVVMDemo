using System;
using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Components.ScheduledTasks.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Components.ScheduledTasks {
	/// <summary>
	/// 默认的任务提供器
	/// </summary>
	[ExportMany]
	public class DefaultScheduledTaskProvider : IScheduledTaskProvider {
		/// <summary>
		/// 查找在Ioc注册的IScheduledTask
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IScheduledTask> GetTasks() {
			return ZKWeb.Application.Ioc.ResolveMany<IScheduledTask>();
		}
	}
}
