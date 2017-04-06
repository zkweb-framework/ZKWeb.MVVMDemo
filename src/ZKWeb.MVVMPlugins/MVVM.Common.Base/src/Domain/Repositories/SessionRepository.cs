using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories {
	/// <summary>
	/// 会话的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionRepository : RepositoryBase<Session, Guid> { }
}
