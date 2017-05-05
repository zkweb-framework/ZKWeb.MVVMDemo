using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.TypeTraits;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Filters
{
    /// <summary>
    /// 根据数据所属用户过滤查询和操作
    /// </summary>
    [ExportMany]
    public class OwnerFilter : IEntityQueryFilter, IEntityOperationFilter
    {
        /// <summary>
        /// 数据应当属于的用户Id
        /// </summary>
        public Guid ExceptedOwnerId => _exceptedOwnerId.Value;
        protected Lazy<Guid> _exceptedOwnerId;

        /// <summary>
        /// 初始化
        /// 数据应当属于的用户Id默认等于当前登录用户Id
        /// </summary>
        public OwnerFilter()
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            _exceptedOwnerId = new Lazy<Guid>(() => sessionManager.GetSession().UserId ?? Guid.Empty);
        }

        /// <summary>
        /// 过滤查询
        /// </summary>
        IQueryable<TEntity> IEntityQueryFilter.FilterQuery<TEntity, TPrimaryKey>(
            IQueryable<TEntity> query)
        {
            if (OwnerTypeTrait<TEntity>.HaveOwner)
            {
                query = query.Where(e => ((IHaveOwner)e).Owner.Id == ExceptedOwnerId);
            }
            return query;
        }

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        Expression<Func<TEntity, bool>> IEntityQueryFilter.FilterPredicate<TEntity, TPrimaryKey>(
            Expression<Func<TEntity, bool>> predicate)
        {
            if (OwnerTypeTrait<TEntity>.HaveOwner)
            {
                var paramExpr = predicate.Parameters[0];
                var memberExpr = Expression.Property(
                    Expression.Property(paramExpr, nameof(IHaveOwner.Owner)),
                    nameof(IEntity<Guid>.Id));
                var body = Expression.AndAlso(
                    predicate.Body,
                    Expression.Equal(memberExpr, Expression.Constant(ExceptedOwnerId)));
                predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
            }
            return predicate;
        }

        /// <summary>
        /// 保存时检查所属用户
        /// </summary>
        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (!OwnerTypeTrait<TEntity>.HaveOwner)
            {
                return;
            }
            var e = ((IHaveOwner)entity);
            if (e.Owner == null && ExceptedOwnerId == Guid.Empty)
            {
                // 未登陆用户保存数据，不需要设置
            }
            else if (e.Owner == null && ExceptedOwnerId != Guid.Empty)
            {
                // 已登陆用户保存数据，设置所属用户，注意这里会受查询过滤器的影响
                var repository = ZKWeb.Application.Ioc.Resolve<IRepository<User, Guid>>();
                var user = repository.Get(u => u.Id == ExceptedOwnerId);
                if (user == null)
                {
                    throw new BadRequestException("Set entity owner failed, user not found");
                }
                e.Owner = user;
            }
            else if (e.Owner != null && e.Owner.Id != ExceptedOwnerId)
            {
                // 已登陆用户保存数据，但数据不属于这个用户
                throw new ForbiddenException(
                    new T("Action require the ownership of {0}: {1}",
                    new T(typeof(TEntity).Name), entity.Id));
            }
        }

        /// <summary>
        /// 删除时检查所属用户
        /// </summary>
        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (!OwnerTypeTrait<TEntity>.HaveOwner)
            {
                return;
            }
            var e = ((IHaveOwner)entity);
            if (e.Owner == null)
            {
                // 删除没有所属用户的数据，不需要拦截
            }
            else if (e.Owner != null && e.Owner.Id != ExceptedOwnerId)
            {
                // 已登陆用户删除数据，但数据不属于这个用户
                throw new ForbiddenException(
                    new T("Action require the ownership of {0}: {1}",
                    new T(typeof(TEntity).Name), entity.Id));
            }
        }
    }
}
