using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs.Attributes;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs {
	/// <summary>
	/// 网站设置
	/// </summary>
	[GenericConfig("Common.Organization.WebsiteSettings", CacheTime = 15)]
	public class WebsiteSettings {
		/// <summary>
		/// 网站名称
		/// </summary>
		public string WebsiteName { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public WebsiteSettings() {
			WebsiteName = "ZKWeb MVVM Demo";
		}
	}
}
