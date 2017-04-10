using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Repositories {
	/// <summary>
	/// 通用配置的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericConfigRepository : RepositoryBase<GenericConfig, string> { }
}
