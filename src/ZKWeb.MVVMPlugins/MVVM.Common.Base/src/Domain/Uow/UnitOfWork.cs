using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow {
	/// <summary>
	/// 工作单元
	/// 工作单元用于在一个区域中共享数据库上下文和事务
	/// 工作单元支持过滤器，这是对框架中的数据事件的补充，但不同的是
	/// - 工作单元过滤器用于处理拥有某一特征(例如有创建时间)的所有实体，可以在一定的范围内启用和禁用
	/// - 数据事件用于处理某一类型的实体，全局一直有效且不能禁用
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UnitOfWork : IUnitOfWork {
		/// <summary>
		/// 同一个工作单元区域使用的数据
		/// </summary>
		private class ScopeData : IDisposable {
			/// <summary>
			/// 数据库上下文
			/// </summary>
			public IDatabaseContext Context { get; set; }
			/// <summary>
			/// 默认的查询过滤器
			/// </summary>
			public IList<IEntityQueryFilter> QueryFilters { get; set; }
			/// <summary>
			/// 默认的操作过滤器
			/// </summary>
			public IList<IEntityOperationFilter> OperationFilters { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public ScopeData() {
				var databaseManager = ZKWeb.Application.Ioc.Resolve<DatabaseManager>();
				Context = databaseManager.CreateContext();
				QueryFilters = ZKWeb.Application.Ioc.ResolveMany<IEntityQueryFilter>().ToList();
				OperationFilters = ZKWeb.Application.Ioc.ResolveMany<IEntityOperationFilter>().ToList();
			}

			/// <summary>
			/// 释放数据
			/// </summary>
			~ScopeData() {
				Dispose();
			}

			/// <summary>
			/// 释放数据
			/// </summary>
			public void Dispose() {
				Context?.Dispose();
				Context = null;
			}
		}

		/// <summary>
		/// 同一个工作单元区域使用的数据
		/// </summary>
		private ThreadLocal<ScopeData> Data { get; set; }

		/// <summary>
		/// 当前的数据库上下文
		/// </summary>
		public IDatabaseContext Context {
			get {
				var context = Data.Value?.Context;
				if (context == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				return context;
			}
		}

		/// <summary>
		/// 当前的查询过滤器列表
		/// </summary>
		public IList<IEntityQueryFilter> QueryFilters {
			get {
				var filters = Data.Value?.QueryFilters;
				if (filters == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				return filters;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException("value");
				} else if (Data.Value == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				Data.Value.QueryFilters = value;
			}
		}

		/// <summary>
		/// 当前的保存过滤器列表
		/// </summary>
		public IList<IEntityOperationFilter> OperationFilters {
			get {
				var filters = Data.Value?.OperationFilters;
				if (filters == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				return filters;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException("value");
				} else if (Data.Value == null) {
					throw new InvalidOperationException("Please call Scope() first");
				}
				Data.Value.OperationFilters = value;
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public UnitOfWork() {
			Data = new ThreadLocal<ScopeData>();
		}

		/// <summary>
		/// 在指定的范围内使用工作单元
		/// 最外层的工作单元负责创建和销毁数据
		/// </summary>
		/// <returns></returns>
		public IDisposable Scope() {
			var isRootUow = Data.Value == null;
			if (isRootUow) {
				var data = new ScopeData();
				Data.Value = data;
				return new SimpleDisposable(() => {
					data.Dispose();
					Data.Value = null;
				});
			}
			return new SimpleDisposable(() => { });
		}
	}
}
