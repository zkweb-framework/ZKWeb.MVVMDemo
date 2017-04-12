using System;
using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// Angular模块的脚本生成器
	/// </summary>
	[ExportMany]
	public class ModuleScriptGenerator {
		/// <summary>
		/// 生成模块脚本
		/// </summary>
		public virtual string GenerateScript(
			ISet<Type> generatedDtoTypes,
			ISet<IApplicationService> generatedApplicationServices,
			ISet<string> generatedTranslationLanguages) {
			return "";
		}
	}
}
