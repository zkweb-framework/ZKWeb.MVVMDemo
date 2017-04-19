using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos {
	/// <summary>
	/// 表格搜索请求
	/// </summary>
	[Description("表格搜索请求")]
	public class GridSearchRequestDto : IInputDto {
		[Description("关键字")]
		public string Keyword { get; set; }
		[Description("当前页")]
		public long Page { get; set; }
		[Description("单页数量")]
		public long Limit { get; set; }
		[Description("排序字段")]
		public string OrderBy { get; set; }
		[Description("是否升序")]
		public bool Ascending { get; set; }
		[Description("列过滤条件")]
		public IDictionary<string, object> ColumnFilters { get; set; }

		public GridSearchRequestDto() {
			ColumnFilters = new Dictionary<string, object>();
		}
	}
}
