using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// 应用服务的脚本生成器
	/// </summary>
	[ExportMany]
	public class ServiceScriptGenerator {
		/// <summary>
		/// 根据应用服务生成脚本
		/// </summary>
		public virtual string GenerateScript(IApplicationService service) {
			return "";
		}
	}
}
