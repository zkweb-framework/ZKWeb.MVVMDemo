using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 网站管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("网站管理服务")]
	public class WebsiteManageService : ApplicationServiceBase {
		private IList<ICacheCleaner> _cacheCleaners;
		private PluginManager _pluginManager;

		public WebsiteManageService(
			IEnumerable<ICacheCleaner> cacheCleaners,
			PluginManager pluginManager) {
			_cacheCleaners = cacheCleaners.ToList();
			_pluginManager = pluginManager;
		}

		[Description("清理缓存")]
		[CheckPrivilege(typeof(ICanUseAdminPanel))]
		public ActionResponseDto ClearCache() {
			foreach (var cleaner in _cacheCleaners) {
				cleaner.ClearCache();
			}
			GC.Collect();
			return ActionResponseDto.CreateSuccess("Clear Cache Successfully");
		}

		[Description("获取网站信息")]
		[CheckPrivilege(typeof(ICanUseAdminPanel))]
		public WebsiteInfoOutputDto GetWebsiteInfo() {
			var websiteInfo = new WebsiteInfoOutputDto();
			websiteInfo.ZKWebVersion = ZKWeb.Application.Version.ToString();
			websiteInfo.ZKWebFullVersion = ZKWeb.Application.FullVersion.ToString();
			websiteInfo.MemoryUsage = FileUtils.GetSizeDisplayName(SystemUtils.GetUsedMemoryBytes());
			foreach (var plugin in _pluginManager.Plugins) {
				websiteInfo.Plugins.Add(new PluginInfoOutputDto() {
					DirectoryName = plugin.DirectoryName(),
					Name = new T(plugin.Name),
					Version = plugin.VersionObject().ToString(),
					FullVersion = plugin.Version,
					Description = new T(plugin.Description)
				});
			}
			return websiteInfo;
		}
	}
}
