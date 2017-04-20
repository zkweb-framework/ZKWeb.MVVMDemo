using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Translates.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Translates {
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
			Translates = new Dictionary<string, string>()
			{
				{ "Submit", "提交" },
				{ "{0} is required", "请填写{0}" },
				{ "Length of {0} must not less than {1}", "{0}的长度必须不少于{1}" },
				{ "Length of {0} must not greater than {1}", "{0}的长度必须不大于{1}" },
				{ "Length of {0} must between {1} and {2}", "{0}的长度必须在{1}和{2}之间" },
				{ "Format of {0} is incorrect", "{0}的格式不正确" },
				{ "Loading", "加载中" },
				{ "Load Failed", "加载失败" },
			};
		}
	}
}
