using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Domain.Services
{
    /// <summary>
    /// 示例数据管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ExampleDataManager : DomainServiceBase<ExampleData, Guid>
    {
    }
}
