using AutoMapper;
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
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.TaskScheduler.src.Domain.Entities;
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
		private GenericConfigManager _configManager;

		public WebsiteManageService(
			IEnumerable<ICacheCleaner> cacheCleaners,
			PluginManager pluginManager,
			GenericConfigManager configManager) {
			_cacheCleaners = cacheCleaners.ToList();
			_pluginManager = pluginManager;
			_configManager = configManager;
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

		[Description("获取网站信息")]
		[CheckPrivilege(typeof(ICanUseAdminPanel))]
		public WebsiteSettingsDto GetWebsiteSettings() {
			var settings = _configManager.GetData<WebsiteSettings>();
			var dto = Mapper.Map<WebsiteSettingsDto>(settings);
			return dto;
		}

		[Description("保存网站信息")]
		[CheckPrivilege(typeof(IAmAdmin), "Settings:WebsiteSettings")]
		public ActionResponseDto SaveWebsiteSettings(WebsiteSettingsDto dto) {
			var settings = Mapper.Map<WebsiteSettings>(dto);
			_configManager.PutData(settings);
			return ActionResponseDto.CreateSuccess("Saved Successfully");
		}

		[Description("搜索定时任务")]
		[CheckPrivilege(typeof(IAmAdmin), "ScheduledTask:View")]
		public GridSearchResponseDto SearchScheduledTasks(GridSearchRequestDto request) {
			return request.BuildResponse<ScheduledTask, string>()
				.FilterKeywordWith(t => t.Id)
				.ToResponse<ScheduledTaskOutputDto>();
		}

		[Description("搜索定时任务记录")]
		[CheckPrivilege(typeof(IAmAdmin), "ScheduledTask:View")]
		public GridSearchResponseDto SearchScheduledTaskLogs(GridSearchRequestDto request) {
			return request.BuildResponse<ScheduledTaskLog, Guid>()
				.FilterKeywordWith(t => t.Task.Id)
				.FilterKeywordWith(t => t.Error)
				.FilterColumnWith(
					nameof(ScheduledTaskLogOutputDto.TaskId),
					(c, q) => q.Where(t => t.Task.Id.Contains((string)c.Value)))
				.ToResponse<ScheduledTaskLogOutputDto>();
		}
	}
}
