using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases {
	/// <summary>
	/// 领域服务的基础类
	/// </summary>
	public abstract class DomainServiceBase : IDomainService {
		/// <summary>
		/// 获取工作单元
		/// </summary>
		protected virtual IUnitOfWork UnitOfWork {
			get { return ZKWeb.Application.Ioc.Resolve<IUnitOfWork>(); }
		}
	}

	/// <summary>
	/// 领域服务的基础类
	/// 提供一系列基础功能
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class DomainServiceBase<TEntity, TPrimaryKey> :
		DomainServiceBase,
		IDomainService<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 获取仓储
		/// </summary>
		protected virtual IRepository<TEntity, TPrimaryKey> Repository {
			get { return ZKWeb.Application.Ioc.Resolve<IRepository<TEntity, TPrimaryKey>>(); }
		}

		/// <summary>
		/// 根据主键获取实体
		/// </summary>
		public virtual TEntity Get(TPrimaryKey id) {
			var expr = ExpressionUtils.MakeMemberEqualiventExpression<TEntity>("Id", id);
			using (UnitOfWork.Scope()) {
				return Repository.Get(expr);
			}
		}

		/// <summary>
		/// 根据条件获取实体
		/// </summary>
		public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate) {
			using (UnitOfWork.Scope()) {
				return Repository.Get(predicate);
			}
		}

		/// <summary>
		/// 根据条件获取实体列表
		/// </summary>
		public virtual IList<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate = null) {
			using (UnitOfWork.Scope()) {
				var query = Repository.Query();
				if (predicate != null) {
					query = query.Where(predicate);
				}
				return query.ToList();
			}
		}

		/// <summary>
		/// 根据过滤函数获取实体列表
		/// </summary>
		public virtual TResult GetMany<TResult>(
			Func<IQueryable<TEntity>, TResult> fetch) {
			using (UnitOfWork.Scope()) {
				return fetch(Repository.Query());
			}
		}

		/// <summary>
		/// 计算符合条件的实体数量
		/// </summary>
		public long Count(Expression<Func<TEntity, bool>> predicate) {
			using (UnitOfWork.Scope()) {
				return Repository.Count(predicate);
			}
		}

		/// <summary>
		/// 保存实体
		/// </summary>
		public virtual void Save(ref TEntity entity, Action<TEntity> update = null) {
			using (UnitOfWork.Scope()) {
				Repository.Save(ref entity, update);
			}
		}

		/// <summary>
		/// 根据主键删除实体
		/// </summary>
		public virtual bool Delete(TPrimaryKey id) {
			var expr = ExpressionUtils.MakeMemberEqualiventExpression<TEntity>("Id", id);
			using (UnitOfWork.Scope()) {
				return Repository.BatchDelete(expr) > 0;
			}
		}

		/// <summary>
		/// 删除实体
		/// </summary>
		public virtual void Delete(TEntity entity) {
			using (UnitOfWork.Scope()) {
				Repository.Delete(entity);
			}
		}

		/// <summary>
		/// 批量标记已删除或未删除
		/// 返回标记的数量，不会实际删除
		/// </summary>
		public virtual long BatchSetDeleted(IEnumerable<TPrimaryKey> ids, bool deleted) {
			var uow = UnitOfWork;
			using (uow.Scope())
			using (uow.DisableQueryFilter(typeof(DeletedFilter))) {
				var entities = Repository.Query().Where(e => ids.Contains(e.Id)).ToList();
				var entitiesRef = entities.AsEnumerable();
				Repository.BatchSave(ref entitiesRef, e => ((IHaveDeleted)e).Deleted = deleted);
				return entities.Count;
			}
		}

		/// <summary>
		/// 批量永久删除
		/// </summary>
		public virtual long BatchDeleteForever(IEnumerable<TPrimaryKey> ids) {
			var uow = UnitOfWork;
			using (uow.Scope())
			using (uow.DisableQueryFilter(typeof(DeletedFilter))) {
				return Repository.BatchDelete(e => ids.Contains(e.Id));
			}
		}
	}
}
