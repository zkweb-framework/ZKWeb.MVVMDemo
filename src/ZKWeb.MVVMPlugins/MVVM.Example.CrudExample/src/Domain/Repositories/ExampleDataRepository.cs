using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Domain.Repositories
{
    /// <summary>
    /// 示例数据的仓储
    /// </summary>
    [ExportMany]
    public class ExampleDataRepository : RepositoryBase<ExampleData, Guid>
    {
        /// <summary>
        /// 查询时包含关联数据
        /// </summary>
        /// <returns></returns>
        public override IQueryable<ExampleData> Query()
        {
            return base.Query().Include(e => e.OwnerTenant);
        }
    }
}
