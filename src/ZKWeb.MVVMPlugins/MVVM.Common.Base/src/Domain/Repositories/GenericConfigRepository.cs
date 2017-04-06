using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories {
	/// <summary>
	/// 通用配置的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericConfigRepository : RepositoryBase<GenericConfig, string> { }
}
