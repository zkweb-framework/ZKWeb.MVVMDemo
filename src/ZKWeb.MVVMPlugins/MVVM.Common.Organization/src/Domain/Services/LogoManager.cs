using System.Drawing;
using System.IO;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Storage;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// Logo管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class LogoManager : DomainServiceBase {
		/// <summary>
		/// 前台Logo的路径
		/// </summary>
		public static readonly string[] FrontPageLogoPath = new[] { "static", "common.base.images", "logo.png" };
		/// <summary>
		/// 后台Logo的路径
		/// </summary>
		public static readonly string[] AdminPanelLogoPath = new[] { "static", "common.admin.images", "logo.png" };
		/// <summary>
		/// 页面图标
		/// </summary>
		public static readonly string[] FaviconPath = new[] { "static", "favicon.ico" };
		/// <summary>
		/// Logo图片质量
		/// </summary>
		public const int LogoImageQuality = 100;

		/// <summary>
		/// 保存前台Logo
		/// </summary>
		/// <param name="stream">图片的数据流</param>
		public virtual void SaveFrontPageLogo(Stream stream) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile(FrontPageLogoPath);
			using (var image = Image.FromStream(stream))
			using (var fileStream = fileEntry.OpenWrite()) {
				image.SaveAuto(fileStream, Path.GetExtension(fileEntry.Filename), LogoImageQuality);
			}
		}

		/// <summary>
		/// 保存后台Logo
		/// </summary>
		/// <param name="stream">图片的数据流</param>
		public virtual void SaveAdminPanelLogo(Stream stream) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile(AdminPanelLogoPath);
			using (var image = Image.FromStream(stream))
			using (var fileStream = fileEntry.OpenWrite()) {
				image.SaveAuto(fileStream, Path.GetExtension(fileEntry.Filename), LogoImageQuality);
			}
		}

		/// <summary>
		/// 保存页面图标
		/// </summary>
		/// <param name="stream">图片的数据流</param>
		public virtual void SaveFavicon(Stream stream) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile(FaviconPath);
			using (var image = Image.FromStream(stream))
			using (var fileStream = fileEntry.OpenWrite()) {
				image.SaveAuto(fileStream, Path.GetExtension(fileEntry.Filename), LogoImageQuality);
			}
		}

		/// <summary>
		/// 恢复默认的前台Logo
		/// </summary>
		public virtual void RestoreDefaultFrontPageLogo() {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile(FrontPageLogoPath);
			fileEntry.Delete();
		}

		/// <summary>
		/// 恢复默认的后台Logo
		/// </summary>
		public virtual void RestoreDefaultAdminPageLogo() {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile(AdminPanelLogoPath);
			fileEntry.Delete();
		}

		/// <summary>
		/// 恢复默认的页面图标
		/// </summary>
		public virtual void RestoreDefaultFavicon() {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile(FaviconPath);
			fileEntry.Delete();
		}
	}
}
