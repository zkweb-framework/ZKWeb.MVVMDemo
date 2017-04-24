using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos {
	[Description("列过滤模式")]
	public enum GridSearchColumnFilterMatchMode {
		[Description("默认(包含过滤内容)")]
		Default = 0,
		[Description("以过滤内容开始")]
		StartsWith = 1,
		[Description("以过滤内容结束")]
		EndsWith = 2,
		[Description("等于过滤内容")]
		Equals = 3,
		[Description("等于过滤值列表的任意一项")]
		In = 4
	}
}
