using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos
{
    /// <summary>
    /// 表格搜索回应
    /// </summary>
    [Description("表格搜索回应")]
    public class GridSearchResponseDto : IOutputDto
    {
        [Description("数据列表")]
        public IList<object> Result { get; set; }
        [Description("分页前的总数量")]
        public long TotalCount { get; set; }
        [Description("附加数据")]
        public object Extra { get; set; }

        public GridSearchResponseDto()
        {
            Result = new List<object>();
        }

        public GridSearchResponseDto(long totalCount, IList<object> result)
        {
            TotalCount = totalCount;
            Result = result;
        }
    }
}
