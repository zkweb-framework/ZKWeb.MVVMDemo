using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.TypeTraits;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Filters {
	/// <summary>
	/// 根据数据所属租户过滤查询和操作
	/// </summary>
	[ExportMany]
	public class OwnerTenantFilter : IEntityQueryFilter, IEntityOperationFilter {
		/// <summary>
		/// 正在使用的用户Id
		/// </summary>
		public Guid UsingUserId { get; set; }
		/// <summary>
		/// 正在使用的租户Id
		/// </summary>
		public Guid UsingTenantId { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OwnerTenantFilter() {
			var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
			UsingUserId = sessionManager.GetSession().UserId ?? Guid.Empty;
			UsingTenantId = sessionManager.GetSession().TenantId ?? Guid.Empty;
		}

		/// <summary>
		/// 过滤查询
		/// </summary>
		IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
			IQueryable<TEntity> query) {
			if (OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				// 用户已登录, 且是租户时, 按租户过滤
				// 用户未登录, 且是租户时, 按租户过滤
				// 用户已登录，且是房东时, 不过滤
				// 用户未登录, 且无租户时, 不返回任何数据
				if (UsingUserId == Guid.Empty && UsingTenantId == Guid.Empty) {
					query = query.Take(0);
				} else if (UsingTenantId != Guid.Empty) {
					query = query.Where(e =>
						((IHaveOwnerTenant)e).OwnerTenant != null &&
						((IHaveOwnerTenant)e).OwnerTenant.Id == UsingTenantId);
				}
			}
			return query;
		}

		/// <summary>
		/// 过滤查询条件
		/// </summary>
		Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
			Expression<Func<TEntity, bool>> predicate) {
			if (OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				// 用户已登录, 且是租户时, 按租户过滤
				// 用户未登录, 且是租户时, 按租户过滤
				// 用户已登录，且是房东时, 不过滤
				// 用户未登录, 且无租户时, 不返回任何数据
				if (UsingUserId == Guid.Empty && UsingTenantId == Guid.Empty) {
					predicate = _ => false;
				} else if (UsingTenantId != Guid.Empty) {
					var paramExpr = predicate.Parameters[0];
					var ownerTanantExpr = Expression.Property(
						paramExpr, nameof(IHaveOwnerTenant.OwnerTenant));
					var ownerTanantIdExpr = Expression.Property(
						ownerTanantExpr, nameof(IEntity<Guid>.Id));
					var body = Expression.AndAlso(
						predicate.Body,
						Expression.AndAlso(
							Expression.NotEqual(ownerTanantExpr, Expression.Constant(null)),
							Expression.Equal(ownerTanantIdExpr, Expression.Constant(UsingTenantId))));
					predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
				}
			}
			return predicate;
		}

		/// <summary>
		/// 保存时检查所属租户
		/// </summary>
		void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity) {
			if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				return;
			}
			// 用户已登录, 且是租户时, 检查并设置数据的租户
			// 用户未登录, 且是租户时, 检查并设置数据的租户
			// 用户已登录，且是房东时, 不检查且不修改数据的租户
			// 用户未登录, 且无租户时, 不允许保存数据
			var e = ((IHaveOwnerTenant)entity);
			bool allowSave;
			if (UsingUserId == Guid.Empty && UsingTenantId == Guid.Empty) {
				allowSave = false;
			} else if (UsingTenantId != Guid.Empty) {
				if (e.OwnerTenant == null) {
					// 设置数据所属租户
					var repository = ZKWeb.Application.Ioc.Resolve<IRepository<Tenant, Guid>>();
					var tenant = repository.Get(u => u.Id == UsingTenantId);
					if (tenant == null) {
						throw new BadRequestException(new T("Set entity owner tenant failed, tenant not found"));
					}
					e.OwnerTenant = tenant;
					allowSave = true;
				} else if (e.OwnerTenant.Id == UsingTenantId) {
					// 检查数据所属租户
					allowSave = true;
				} else {
					allowSave = false;
				}
			} else {
				allowSave = true;
			}
			// 检查失败时抛出错误
			if (!allowSave) {
				throw new ForbiddenException(
					new T("Action require the tennat ownership of {0}: {1}",
					new T(typeof(TEntity).Name), entity.Id));
			}
		}

		/// <summary>
		/// 删除时检查所属租户
		/// </summary>
		void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) {
			if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				return;
			}
			// 用户已登录, 且是租户时, 检查数据的租户
			// 用户未登录, 且是租户时, 检查数据的租户
			// 用户已登录，且是房东时, 不检查数据的租户
			// 用户未登录, 且无租户时, 不允许删除数据
			var e = ((IHaveOwnerTenant)entity);
			bool allowDelete;
			if (UsingUserId == Guid.Empty && UsingTenantId == Guid.Empty) {
				allowSave = false;
			} else if (UsingTenantId != Guid.Empty) {
				if (e.OwnerTenant == null) {
				}
			}

			if ((e.OwnerTenant == null && UsingTenantId != Guid.Empty) ||
		(e.OwnerTenant != null && UsingTenantId != Guid.Empty &&
		e.OwnerTenant.Id != UsingTenantId)) {
				// 其他租户删除主租户的数据
				// 或者非主租户删除其他租户的数据
				throw new ForbiddenException(
					new T("Action require the tennat ownership of {0}: {1}",
					new T(typeof(TEntity).Name), entity.Id));
			}
		}
	}
}
