using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces {
	/// <summary>
	/// 工作单元的接口
	/// </summary>
	public interface IUnitOfWork {
		/// <summary>
		/// 当前的数据库上下文
		/// 不存在时抛出错误
		/// </summary>
		IDatabaseContext Context { get; }

		/// <summary>
		/// 当前的查询过滤器列表
		/// 不存在时抛出错误
		/// </summary>
		IList<IEntityQueryFilter> QueryFilters { get; set; }

		/// <summary>
		/// 当前的操作过滤器列表
		/// 不存在时抛出错误
		/// </summary>
		IList<IEntityOperationFilter> OperationFilters { get; set; }

		/// <summary>
		/// 在指定的范围内使用工作单元
		/// 工作单元中可以使用相同的上下文和过滤器，并且和其他工作单元隔离
		/// 这个函数可以嵌套使用，嵌套使用时都使用最上层的数据库上下文
		/// </summary>
		/// <returns></returns>
		IDisposable Scope();
	}
}
