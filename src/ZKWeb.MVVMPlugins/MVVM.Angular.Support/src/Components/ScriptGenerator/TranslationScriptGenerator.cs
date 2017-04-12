using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// 翻译的脚本生成器
	/// </summary>
	[ExportMany]
	public class TranslationScriptGenerator {
		/// <summary>
		/// 根据翻译的语言生成脚本
		/// </summary>
		public virtual string GenerateScript(string language) {
			return "";
		}
	}
}
