using AutoMapper;
using System.Collections.Generic;
using ZKWeb.Plugin;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src {
	/// <summary>
	/// 插件载入时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 自动注册AutoMapper的映射设置
		/// </summary>
		public Plugin(IEnumerable<Profile> profiles) {
			Mapper.Initialize(cfg => profiles.ForEach(p => cfg.AddProfile(p)));
		}
	}
}
