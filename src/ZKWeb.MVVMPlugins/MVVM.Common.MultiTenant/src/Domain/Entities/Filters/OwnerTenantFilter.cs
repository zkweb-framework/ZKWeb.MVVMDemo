using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.TypeTraits;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Filters {
	/// <summary>
	/// 根据数据所属租户过滤查询和操作
	/// </summary>
	[ExportMany]
	public class OwnerTenantFilter : IEntityQueryFilter, IEntityOperationFilter {
		/// <summary>
		/// 当前的租户
		/// </summary>
		public Tenant UsingTenant { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OwnerTenantFilter() {
			var tenantManager = Application.Ioc.Resolve<TenantManager>();
			UsingTenant = tenantManager.GetTenant();
		}

		/// <summary>
		/// 过滤查询
		/// </summary>
		IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
			IQueryable<TEntity> query) {
			if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				return query;
			}
			// 主租户不过滤数据
			if (UsingTenant != null && UsingTenant.IsMaster) {
				return query;
			}
			// 按租户过滤数据
			if (UsingTenant == null) {
				return query.Where(e =>
					((IHaveOwnerTenant)e).OwnerTenant == null);
			} else {
				return query.Where(e =>
				   ((IHaveOwnerTenant)e).OwnerTenant != null &&
				   ((IHaveOwnerTenant)e).OwnerTenant.Id == UsingTenant.Id);
			}
		}

		/// <summary>
		/// 过滤查询条件
		/// </summary>
		Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
			Expression<Func<TEntity, bool>> predicate) {
			if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				return predicate;
			}
			// 主租户不过滤数据
			if (UsingTenant != null && UsingTenant.IsMaster) {
				return predicate;
			}
			// 按租户过滤数据
			if (UsingTenant == null) {
				var paramExpr = predicate.Parameters[0];
				var ownerTanantExpr = Expression.Property(
					paramExpr, nameof(IHaveOwnerTenant.OwnerTenant));
				var ownerTanantIdExpr = Expression.Property(
					ownerTanantExpr, nameof(IEntity<Guid>.Id));
				var body = Expression.AndAlso(
					predicate.Body,
					Expression.Equal(ownerTanantExpr, Expression.Constant(null)));
				predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
			} else {
				var paramExpr = predicate.Parameters[0];
				var ownerTanantExpr = Expression.Property(
					paramExpr, nameof(IHaveOwnerTenant.OwnerTenant));
				var ownerTanantIdExpr = Expression.Property(
					ownerTanantExpr, nameof(IEntity<Guid>.Id));
				var body = Expression.AndAlso(
					predicate.Body,
					Expression.AndAlso(
						Expression.NotEqual(ownerTanantExpr, Expression.Constant(null)),
						Expression.Equal(ownerTanantIdExpr, Expression.Constant(UsingTenant.Id))));
				predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
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
			var e = ((IHaveOwnerTenant)entity);
			if (e.OwnerTenant == null) {
				// 设置数据的租户
				if (UsingTenant != null) {
					var repository = Application.Ioc.Resolve<IRepository<Tenant, Guid>>();
					e.OwnerTenant = repository.Get(u => u.Id == UsingTenant.Id);
				}
			} else {
				// 检查数据的租户是否一致，主租户不要求一致
				if (UsingTenant == null ||
					(!UsingTenant.IsMaster && UsingTenant.Id != e.OwnerTenant.Id)) {
					throw new ForbiddenException(
					new T("Action require the tennat ownership of {0}: {1}",
					new T(typeof(TEntity).Name), entity.Id));
				}
			}
		}

		/// <summary>
		/// 删除时检查所属租户
		/// </summary>
		void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) {
			if (!OwnerTenantTypeTrait<TEntity>.HaveOwnerTenant) {
				return;
			}
			var e = ((IHaveOwnerTenant)entity);
			if (e.OwnerTenant != null) {
				// 检查数据的租户是否一致，主租户不要求一致
				if (UsingTenant == null ||
					(!UsingTenant.IsMaster && UsingTenant.Id != e.OwnerTenant.Id)) {
					throw new ForbiddenException(
						new T("Action require the tennat ownership of {0}: {1}",
						new T(typeof(TEntity).Name), entity.Id));
				}
			}
		}
	}
}
