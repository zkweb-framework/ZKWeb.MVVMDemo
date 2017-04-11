using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// 角色管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RoleManager : DomainServiceBase<Role, Guid> { }
}
