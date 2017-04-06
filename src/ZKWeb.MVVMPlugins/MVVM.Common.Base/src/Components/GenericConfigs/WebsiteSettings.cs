using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.GenericConfigs.Attributes;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.GenericConfigs {
	/// <summary>
	/// 网站设置
	/// </summary>
	[GenericConfig("Common.Base.WebsiteSettings", CacheTime = 15)]
	public class WebsiteSettings {
		/// <summary>
		/// 网站名称
		/// </summary>
		public string WebsiteName { get; set; }
		/// <summary>
		/// 网页标题的默认格式，默认是 {title} - {websiteName}
		/// </summary>
		public string DocumentTitleFormat { get; set; }
		/// <summary>
		/// 页面关键词
		/// </summary>
		public string PageKeywords { get; set; }
		/// <summary>
		/// 页面描述
		/// </summary>
		public string PageDescription { get; set; }
		/// <summary>
		/// 版权信息
		/// </summary>
		public string CopyrightText { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public WebsiteSettings() {
			WebsiteName = "ZKWeb Default Website";
			DocumentTitleFormat = "{title} - {websiteName}";
			PageKeywords = "";
			PageDescription = "";
			CopyrightText = "Copyright © 2016~2017 ZKWeb All rights reserved";
		}
	}
}
