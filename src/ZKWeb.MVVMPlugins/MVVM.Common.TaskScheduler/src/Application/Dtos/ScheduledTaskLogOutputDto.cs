using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Application.Dtos {
	[Description("任务执行记录的传出信息")]
	public class ScheduledTaskLogOutputDto : IOutputDto {
		[Description("记录Id")]
		public Guid Id { get; set; }
		[Description("任务Id")]
		public string TaskId { get; set; }
		[Description("执行时间")]
		public string CreateTime { get; set; }
		[Description("是否成功")]
		public bool Success { get; set; }
		[Description("错误信息")]
		public string Error { get; set; }
	}
}
