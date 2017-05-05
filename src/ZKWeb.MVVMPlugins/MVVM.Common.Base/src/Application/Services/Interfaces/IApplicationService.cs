using System.Collections.Generic;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Structs;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces
{
    /// <summary>
    /// 应用服务的接口
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// 获取Api函数列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<ApplicationServiceApiMethodInfo> GetApiMethods();
    }
}
