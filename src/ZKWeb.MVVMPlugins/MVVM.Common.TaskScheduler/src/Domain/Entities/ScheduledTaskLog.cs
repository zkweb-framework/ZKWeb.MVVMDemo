using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Entities
{
    /// <summary>
    /// 定时任务的执行记录
    /// </summary>
    [ExportMany]
    public class ScheduledTaskLog :
        IEntity<Guid>,
        IHaveCreateTime,
        IEntityMappingProvider<ScheduledTaskLog>
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 关联的任务
        /// </summary>
        public virtual ScheduledTask Task { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public virtual bool Success { get; set; }
        /// <summary>
        /// 执行中发生的错误
        /// </summary>
        public virtual string Error { get; set; }

        /// <summary>
        /// 配置数据库结构
        /// </summary>
        public void Configure(IEntityMappingBuilder<ScheduledTaskLog> builder)
        {
            builder.Id(t => t.Id);
            builder.References(t => t.Task, new EntityMappingOptions() { Nullable = false });
            builder.Map(t => t.CreateTime);
            builder.Map(t => t.Success);
            builder.Map(t => t.Error);
        }
    }
}
