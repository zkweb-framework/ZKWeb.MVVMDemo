using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Repositories
{
    /// <summary>
    /// 定时任务的仓储
    /// </summary>
    [ExportMany]
    public class ScheduledTaskRepository : RepositoryBase<ScheduledTask, string>
    {
    }
}
