using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions {
	/// <summary>
	/// 工作单元的扩展函数
	/// </summary>
	public static class IUnitOfWorkExtensions {
		/// <summary>
		/// 包装更新函数
		/// 应用工作单元中的过滤器
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="uow">工作单元</param>
		/// <param name="update">更新函数</param>
		/// <returns></returns>
		public static Action<TEntity> WrapUpdateMethod<TEntity, TPrimaryKey>(
			this IUnitOfWork uow, Action<TEntity> update)
			where TEntity : class, IEntity<TPrimaryKey> {
			return e => {
				foreach (var filter in uow.OperationFilters) {
					filter.FilterSave<TEntity, TPrimaryKey>(e);
				}
				update?.Invoke(e);
			};
		}

		/// <summary>
		/// 包装删除前的函数
		/// 应用工作单元中的过滤器
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="uow">工作单元</param>
		/// <param name="beforeDelete">删除前的函数</param>
		/// <returns></returns>
		public static Action<TEntity> WrapBeforeDeleteMethod<TEntity, TPrimaryKey>(
			this IUnitOfWork uow, Action<TEntity> beforeDelete)
			where TEntity : class, IEntity<TPrimaryKey> {
			return e => {
				foreach (var filter in uow.OperationFilters) {
					filter.FilterDelete<TEntity, TPrimaryKey>(e);
				}
				beforeDelete?.Invoke(e);
			};
		}

		/// <summary>
		/// 包装查询
		/// 应用工作单元中的过滤器
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="uow">工作单元</param>
		/// <param name="query">查询</param>
		/// <returns></returns>
		public static IQueryable<TEntity> WrapQuery<TEntity, TPrimaryKey>(
			this IUnitOfWork uow, IQueryable<TEntity> query)
			where TEntity : class, IEntity<TPrimaryKey> {
			foreach (var filter in uow.QueryFilters) {
				query = filter.FilterQuery<TEntity, TPrimaryKey>(query);
			}
			return query;
		}

		/// <summary>
		/// 包装查询条件
		/// 应用工作单元中的过滤器
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="uow">工作单元</param>
		/// <param name="predicate">查询条件</param>
		/// <returns></returns>
		public static Expression<Func<TEntity, bool>> WrapPredicate<TEntity, TPrimaryKey>(
			this IUnitOfWork uow, Expression<Func<TEntity, bool>> predicate)
			where TEntity : class, IEntity<TPrimaryKey> {
			foreach (var filter in uow.QueryFilters) {
				predicate = filter.FilterPredicate<TEntity, TPrimaryKey>(predicate);
			}
			return predicate;
		}

		/// <summary>
		/// 在一定范围内禁用指定类型的查询过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filterType">查询过滤器类型</param>
		/// <returns></returns>
		public static IDisposable DisableQueryFilter(this IUnitOfWork uow, Type filterType) {
			var oldFilters = uow.QueryFilters;
			var filterTypeInfo = filterType.GetTypeInfo();
			uow.QueryFilters = uow.QueryFilters.Where(
				f => !filterTypeInfo.IsAssignableFrom(f.GetType())).ToList();
			return new SimpleDisposable(() => uow.QueryFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内禁用所有查询过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <returns></returns>
		public static IDisposable DisableAllQueryFilters(this IUnitOfWork uow) {
			var oldFilters = uow.QueryFilters;
			uow.QueryFilters = new List<IEntityQueryFilter>();
			return new SimpleDisposable(() => uow.QueryFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内禁用指定类型的操作过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filterType">过滤器类型</param>
		/// <returns></returns>
		public static IDisposable DisableOperationFilter(this IUnitOfWork uow, Type filterType) {
			var oldFilters = uow.OperationFilters;
			var filterTypeInfo = filterType.GetTypeInfo();
			uow.OperationFilters = uow.OperationFilters.Where(
				f => !filterTypeInfo.IsAssignableFrom(f.GetType())).ToList();
			return new SimpleDisposable(() => uow.OperationFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内禁用所有操作过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <returns></returns>
		public static IDisposable DisableAllOperationFilters(this IUnitOfWork uow) {
			var oldFilters = uow.OperationFilters;
			uow.OperationFilters = new List<IEntityOperationFilter>();
			return new SimpleDisposable(() => uow.OperationFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内启用指定的查询过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filter">查询过滤器</param>
		/// <returns></returns>
		public static IDisposable EnableQueryFilter(
			this IUnitOfWork uow, IEntityQueryFilter filter) {
			var oldFilters = uow.QueryFilters;
			uow.QueryFilters = uow.QueryFilters.ConcatIfNotNull(filter).ToList();
			return new SimpleDisposable(() => uow.QueryFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内启用指定的操作过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filter">操作过滤器</param>
		/// <returns></returns>
		public static IDisposable EnableOperationFilter(
			this IUnitOfWork uow, IEntityOperationFilter filter) {
			var oldFilters = uow.OperationFilters;
			uow.OperationFilters = uow.OperationFilters.ConcatIfNotNull(filter).ToList();
			return new SimpleDisposable(() => uow.OperationFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内启用指定的过滤器
		/// 自动检测过滤器是查询还是操作过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filter">过滤器</param>
		/// <returns></returns>
		public static IDisposable EnableFilter(
			this IUnitOfWork uow, object filter) {
			var oldQueryFilters = uow.QueryFilters;
			var oldOperationFilters = uow.OperationFilters;
			if (filter is IEntityQueryFilter) {
				uow.QueryFilters = uow.QueryFilters
					.ConcatIfNotNull((IEntityQueryFilter)filter).ToList();
			}
			if (filter is IEntityOperationFilter) {
				uow.OperationFilters = uow.OperationFilters
					.ConcatIfNotNull((IEntityOperationFilter)filter).ToList();
			}
			return new SimpleDisposable(() => {
				uow.QueryFilters = oldQueryFilters;
				uow.OperationFilters = oldOperationFilters;
			});
		}

		/// <summary>
		/// 在一定范围内禁用指定的过滤器
		/// 自动检测过滤器是查询还是操作过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filterType">过滤器类型</param>
		/// <returns></returns>
		public static IDisposable DisableFilter(
			this IUnitOfWork uow, Type filterType) {
			var oldQueryFilters = uow.QueryFilters;
			var oldOperationFilters = uow.OperationFilters;
			var filterTypeInfo = filterType.GetTypeInfo();
			uow.QueryFilters = uow.QueryFilters.Where(
				f => !filterTypeInfo.IsAssignableFrom(f.GetType())).ToList();
			uow.OperationFilters = uow.OperationFilters.Where(
				f => !filterTypeInfo.IsAssignableFrom(f.GetType())).ToList();
			return new SimpleDisposable(() => {
				uow.QueryFilters = oldQueryFilters;
				uow.OperationFilters = oldOperationFilters;
			});
		}
	}
}
