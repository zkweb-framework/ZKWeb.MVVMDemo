using System.Collections.Generic;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Components.ScheduledTasks.Interfaces
{
    /// <summary>
    /// 定时任务提供器
    /// </summary>
    public interface IScheduledTaskProvider
    {
        /// <summary>
        /// 获取定时任务
        /// </summary>
        /// <returns></returns>
        IEnumerable<IScheduledTask> GetTasks();
    }
}
