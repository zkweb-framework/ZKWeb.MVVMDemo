using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Admin.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// 角色管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserRoleManager : DomainServiceBase<UserRole, Guid> { }
}
