using System;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces
{
    /// <summary>
    /// 查询过滤器
    /// </summary>
    public interface IEntityQueryFilter
    {
        /// <summary>
        /// 过滤查询
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        IQueryable<TEntity> FilterQuery<TEntity, TPrimaryKey>(
            IQueryable<TEntity> query)
            where TEntity : class, IEntity<TPrimaryKey>;

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> FilterPredicate<TEntity, TPrimaryKey>(
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IEntity<TPrimaryKey>;
    }
}
