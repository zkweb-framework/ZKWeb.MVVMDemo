using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Translates.Bases {
	/// <summary>
	/// 基于词典的翻译提供器的基类
	/// </summary>
	public abstract class DictionaryTranslationProviderBase : ITranslateProvider {
		/// <summary>
		/// 支持的语言列表
		/// </summary>
		public ISet<string> Codes { get; protected set; }
		/// <summary>
		/// 翻译文本的词典
		/// </summary>
		public IDictionary<string, string> Translates { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public DictionaryTranslationProviderBase() {
			Codes = new HashSet<string>();
			Translates = new Dictionary<string, string>();
		}

		/// <summary>
		/// 判断是否能翻译该语言
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		/// <summary>
		/// 翻译文本
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
