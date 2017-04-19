using System;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.RequestHandlers {
	/// <summary>
	/// 根据传入的Http头设置语言和时区
	/// </summary>
	[ExportMany]
	public class LocaleRequestHandler : IHttpRequestPreHandler {
		/// <summary>
		/// 客户端传入语言使用的Http头
		/// </summary>
		public const string LanguageHeader = "X-ZKWeb-Language";
		/// <summary>
		/// 客户端传入时区使用的Http头
		/// </summary>
		public const string TimezoneHeader = "X-ZKWeb-Timezone";

		/// <summary>
		/// 根据传入的Http头设置语言和时区
		/// </summary>
		public void OnRequest() {
			var request = HttpManager.CurrentContext.Request;
			var language = request.GetHeader(LanguageHeader);
			var timezone = request.GetHeader(TimezoneHeader);
			if (!string.IsNullOrEmpty(language)) {
				LocaleUtils.SetThreadLanguage(language);
			}
			if (!string.IsNullOrEmpty(timezone)) {
				LocaleUtils.SetThreadTimezone(timezone);
			}
		}
	}
}
