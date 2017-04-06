using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces {
	/// <summary>
	/// 领域服务的接口
	/// </summary>
	public interface IDomainService { }

	/// <summary>
	/// 领域服务的接口
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public interface IDomainService<TEntity, TPrimaryKey> : IDomainService
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 根据主键获取实体
		/// </summary>
		/// <param name="id">主键值</param>
		/// <returns></returns>
		TEntity Get(TPrimaryKey id);

		/// <summary>
		/// 根据条件获取实体
		/// </summary>
		/// <param name="predicate">条件</param>
		/// <returns></returns>
		TEntity Get(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 根据条件获取实体列表
		/// </summary>
		/// <param name="predicate">条件，等于null时获取所有实体</param>
		/// <returns></returns>
		IList<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate = null);

		/// <summary>
		/// 根据查询函数获取实体列表
		/// </summary>
		/// <param name="fetch">查询函数</param>
		/// <returns></returns>
		TResult GetMany<TResult>(Func<IQueryable<TEntity>, TResult> fetch);

		/// <summary>
		/// 计算符合条件的实体数量
		/// </summary>
		/// <param name="predicate">条件</param>
		/// <returns></returns>
		long Count(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 保存实体
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="update">更新函数</param>
		void Save(ref TEntity entity, Action<TEntity> update = null);

		/// <summary>
		/// 根据主键删除实体
		/// 返回是否找到并删除了实体
		/// </summary>
		/// <param name="id">主键值</param>
		/// <returns></returns>
		bool Delete(TPrimaryKey id);

		/// <summary>
		/// 删除实体
		/// </summary>
		/// <param name="entity">实体</param>
		void Delete(TEntity entity);

		/// <summary>
		/// 批量标记已删除或未删除
		/// 返回标记的数量，不会实际删除
		/// </summary>
		/// <param name="ids">实体Id列表</param>
		/// <param name="deleted">是否已删除</param>
		/// <returns></returns>
		long BatchSetDeleted(IEnumerable<TPrimaryKey> ids, bool deleted);

		/// <summary>
		/// 批量永久删除
		/// 返回实际删除的数量
		/// </summary>
		/// <param name="ids">批量永久删除</param>
		/// <returns></returns>
		long BatchDeleteForever(IEnumerable<TPrimaryKey> ids);
	}
}
