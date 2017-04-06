using System;
using ZKWeb.Storage;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMDemo.Plugins.MVVM.Angular.Support.src.Applications {
	/// <summary>
	/// 负责处理AngularJS相关的请求
	/// </summary>
	[ExportMany]
	public class AngularRouter : IHttpRequestHandler {
		/// <summary>
		/// Api的前缀
		/// TODO: 需要移动到Base插件中
		/// </summary>
		public const string ApiPrefix = "/api/";
		private IFileStorage _fileStorage;

		/// <summary>
		/// 初始化
		/// </summary>
		public AngularRouter(IFileStorage fileStorage) {
			_fileStorage = fileStorage;
		}

		/// <summary>
		/// 处理请求
		/// </summary>
		public void OnRequest() {
			var context = HttpManager.CurrentContext;
			var path = context.Request.Path.Substring(1);
			// 不处理Api请求
			if (path.StartsWith(ApiPrefix)) {
				return;
			}
			// 查找对应的文件
			IFileEntry fileEntry = null;
			if (string.IsNullOrEmpty(path)) {
				// 请求的是首页
				fileEntry = _fileStorage.GetResourceFile("static", "src", "index.html");
			} else {
				// 请求的是子页或资源文件
				fileEntry = _fileStorage.GetResourceFile("static", path);
				if (!fileEntry.Exists) {
					fileEntry = _fileStorage.GetResourceFile("static", "src", path);
				}
				// 不是资源文件，当作子页处理
				if (!fileEntry.Exists) {
					fileEntry = _fileStorage.GetResourceFile("static", "src", "index.html");
				}
			}
			// 返回文件内容
			if (fileEntry.Exists) {
				var result = new FileEntryResult(fileEntry, context.Request.GetIfModifiedSince());
				result.WriteResponse(context.Response);
				context.Response.End();
			}
		}
	}
}
