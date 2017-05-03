using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 网站管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("网站管理服务")]
	public class WebsiteManageService : ApplicationServiceBase {
		private IList<ICacheCleaner> _cacheCleaners;

		public WebsiteManageService(IEnumerable<ICacheCleaner> cacheCleaners) {
			_cacheCleaners = cacheCleaners.ToList();
		}

		[Description("清理缓存")]
		public ActionResponseDto ClearCache() {
			foreach (var cleaner in _cacheCleaners) {
				cleaner.ClearCache();
			}
			GC.Collect();
			return ActionResponseDto.CreateSuccess("Clear Cache Successfully");
		}
	}
}
