using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters {
	/// <summary>
	/// 自动设置实体的更新时间
	/// </summary>
	[ExportMany]
	public class UpdateTimeFilter : IEntityOperationFilter {
		/// <summary>
		/// 自动设置实体的更新时间
		/// </summary>
		void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity) {
			if (entity is IHaveUpdateTime) {
				var et = (IHaveUpdateTime)entity;
				et.UpdateTime = DateTime.UtcNow;
			}
		}

		/// <summary>
		/// 不需要处理删除
		/// </summary>
		void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) { }
	}
}
