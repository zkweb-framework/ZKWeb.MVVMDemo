using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces
{
    /// <summary>
    /// 操作过滤器
    /// </summary>
    public interface IEntityOperationFilter
    {
        /// <summary>
        /// 过滤保存
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="entity">实体对象</param>
        void FilterSave<TEntity, TPrimaryKey>(TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>;

        /// <summary>
        /// 过滤删除
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="entity">实体对象</param>
        void FilterDelete<TEntity, TPrimaryKey>(TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>;
    }
}
