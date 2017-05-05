using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Repositories
{
    /// <summary>
    /// 会话的仓储
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SessionRepository : RepositoryBase<Session, Guid> { }
}
