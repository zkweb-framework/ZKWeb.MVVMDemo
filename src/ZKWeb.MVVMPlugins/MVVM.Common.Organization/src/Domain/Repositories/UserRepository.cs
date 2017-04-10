using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Repositories {
	/// <summary>
	/// 用户的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserRepository : RepositoryBase<User, Guid> { }
}
