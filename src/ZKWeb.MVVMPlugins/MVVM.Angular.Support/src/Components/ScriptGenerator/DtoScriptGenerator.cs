using System;
using System.Reflection;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// 数据传输对象的脚本生成器
	/// </summary>
	[ExportMany]
	public class DtoScriptGenerator {
		/// <summary>
		/// 判断是否应该生成此类型的脚本
		/// </summary>
		/// <returns></returns>
		public virtual bool ShouldGenerate(Type type) {
			var typeInfo = type.GetTypeInfo();
			if (typeInfo.IsClass &&
				typeInfo.Assembly != typeof(string).GetTypeInfo().Assembly) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// 根据对象类型生成脚本
		/// </summary>
		public virtual string GenerateScript(Type type) {
			return "";
		}
	}
}
