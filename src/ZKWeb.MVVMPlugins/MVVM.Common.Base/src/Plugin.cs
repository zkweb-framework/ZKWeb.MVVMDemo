using AutoMapper;
using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ActionParameterProviders;
using ZKWeb.Plugin;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src {
	/// <summary>
	/// 插件载入时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin(IEnumerable<Profile> profiles) {
			// 自动注册AutoMapper的映射设置
			Mapper.Initialize(cfg => profiles.ForEach(p => cfg.AddProfile(p)));
			// 替换默认的参数提供器
			var originalProvider = ZKWeb.Application.Ioc.Resolve<IActionParameterProvider>();
			ZKWeb.Application.Ioc.Unregister<IActionParameterProvider>();
			ZKWeb.Application.Ioc.RegisterInstance<IActionParameterProvider>(
				new ValidatedActionParameterProvider(originalProvider));
		}
	}
}
