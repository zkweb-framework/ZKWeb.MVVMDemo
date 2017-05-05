using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Translates.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany]
	public class zh_CN : DictionaryTranslationProviderBase {
		/// <summary>
		/// 初始化
		/// </summary>
		public zh_CN() {
			Codes = new HashSet<string>() { "zh-CN" };
			Translates = new Dictionary<string, string>() {
				{ "ExampleData", "示例数据" },
				{ "Example Datas", "示例数据" },
				{ "Edit ExampleData", "编辑示例数据" },
				{ "ExampleData Manage", "示例数据管理" }
			};
		}
	}
}
