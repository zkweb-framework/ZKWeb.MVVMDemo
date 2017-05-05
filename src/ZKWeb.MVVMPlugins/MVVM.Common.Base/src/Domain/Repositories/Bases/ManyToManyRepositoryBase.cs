using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Bases;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases
{
    /// <summary>
    /// 多对多的仓储基类
    /// </summary>
    /// <typeparam name="TFrom">来源实体类型</typeparam>
    /// <typeparam name="TTo">目标实体类型</typeparam>
    /// <typeparam name="TEntity">多对多实体类型</typeparam>
    public class ManyToManyRepositoryBase<TFrom, TTo, TEntity> : RepositoryBase<TEntity, Guid>
        where TFrom : class, IEntity
        where TTo : class, IEntity
        where TEntity : ManyToManyEntityBase<TFrom, TTo, TEntity>
    {
        /// <summary>
        /// 查询时包含关联数据
        /// </summary>
        /// <returns></returns>
        public override IQueryable<TEntity> Query()
        {
            return base.Query().Include(x => x.From).Include(x => x.To);
        }
    }
}
