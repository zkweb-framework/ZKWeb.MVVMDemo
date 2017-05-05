using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.GridSearchResponseBuilder;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos
{
    /// <summary>
    /// 表格搜索请求
    /// </summary>
    [Description("表格搜索请求")]
    public class GridSearchRequestDto : IInputDto
    {
        [Description("关键字")]
        public string Keyword { get; set; }
        [Description("当前页，从0开始")]
        public int Page { get; set; }
        [Description("单页数量")]
        public int PageSize { get; set; }
        [Description("排序字段")]
        public string OrderBy { get; set; }
        [Description("是否升序")]
        public bool Ascending { get; set; }
        [Description("列过滤条件")]
        public IList<GridSearchColumnFilter> ColumnFilters { get; set; }
        [Description("附加数据")]
        public object Extra { get; set; }

        public GridSearchRequestDto()
        {
            ColumnFilters = new List<GridSearchColumnFilter>();
        }

        /// <summary>
        /// 开始构建搜索回应
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <returns></returns>
        public GridSearchResponseBuilder<TEntity, TPrimaryKey> BuildResponse<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity, IEntity<TPrimaryKey>
        {
            return new GridSearchResponseBuilder<TEntity, TPrimaryKey>(this);
        }
    }
}
