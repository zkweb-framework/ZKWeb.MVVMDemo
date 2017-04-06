using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces {
	/// <summary>
	/// 仓储的接口
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public interface IRepository<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 查询实体
		/// 受这些过滤器的影响: 查询过滤器
		/// </summary>
		/// <returns></returns>
		IQueryable<TEntity> Query();

		/// <summary>
		/// 获取符合条件的单个实体
		/// 受这些过滤器的影响: 查询过滤器
		/// </summary>
		/// <param name="predicate">条件</param>
		/// <returns></returns>
		TEntity Get(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 计算符合条件的实体数量
		/// 受这些过滤器的影响: 查询过滤器
		/// </summary>
		/// <param name="predicate">条件</param>
		/// <returns></returns>
		long Count(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 添加或更新实体
		/// 受这些过滤器的影响: 操作过滤器
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="update">更新函数</param>
		void Save(ref TEntity entity, Action<TEntity> update);

		/// <summary>
		/// 删除实体
		/// 受这些过滤器的影响: 操作过滤器
		/// </summary>
		/// <param name="entity">实体</param>
		void Delete(TEntity entity);

		/// <summary>
		/// 批量保存实体
		/// 受这些过滤器的影响: 操作过滤器
		/// </summary>
		/// <param name="entities">实体列表</param>
		/// <param name="update">更新函数</param>
		void BatchSave(
			ref IEnumerable<TEntity> entities, Action<TEntity> update = null);

		/// <summary>
		/// 批量更新实体
		/// 受这些过滤器的影响: 查询过滤器, 操作过滤器
		/// </summary>
		/// <param name="predicate">更新条件</param>
		/// <param name="update">更新函数</param>
		/// <returns></returns>
		long BatchUpdate(
			Expression<Func<TEntity, bool>> predicate, Action<TEntity> update);

		/// <summary>
		/// 批量删除实体
		/// 受这些过滤器的影响: 查询过滤器, 操作过滤器
		/// </summary>
		/// <param name="predicate">删除条件</param>
		/// <param name="beforeDelete">删除前的函数</param>
		/// <returns></returns>
		long BatchDelete(
			Expression<Func<TEntity, bool>> predicate, Action<TEntity> beforeDelete = null);
	}
}
