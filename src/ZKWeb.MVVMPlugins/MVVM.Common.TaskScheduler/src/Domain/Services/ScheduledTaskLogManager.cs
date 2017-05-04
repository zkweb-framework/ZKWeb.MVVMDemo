using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Services {
	/// <summary>
	/// 定时任务记录管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScheduledTaskLogManager : DomainServiceBase<ScheduledTaskLog, Guid> {
	}
}
