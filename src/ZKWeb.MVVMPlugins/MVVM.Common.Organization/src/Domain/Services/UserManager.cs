using System;
using System.Drawing;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.Server;
using ZKWeb.Storage;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// 用户管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserManager : DomainServiceBase<User, Guid> {
		/// <summary>
		/// 记住登陆时，保留会话的天数
		/// 默认30天，可通过网站配置指定
		/// </summary>
		public TimeSpan SessionExpireDaysWithRememebrLogin { get; set; }
		/// <summary>
		/// 不记住登陆时，保留会话的天数
		/// 默认1天，可通过网站配置指定
		/// </summary>
		public TimeSpan SessionExpireDaysWithoutRememberLogin { get; set; }
		/// <summary>
		/// 头像宽度
		/// 默认150，可通过网站配置指定
		/// </summary>
		public int AvatarWidth { get; set; }
		/// <summary>
		/// 头像高度
		/// 默认150，可通过网站配置指定
		/// </summary>
		public int AvatarHeight { get; set; }
		/// <summary>
		/// 头像图片质量，默认90
		/// </summary>
		public int AvatarImageQuality { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public UserManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var extra = configManager.WebsiteConfig.Extra;
			SessionExpireDaysWithRememebrLogin = TimeSpan.FromDays(
				extra.GetOrDefault(AdminExtraConfigKeys.SessionExpireDaysWithRememebrLogin, 30));
			SessionExpireDaysWithoutRememberLogin = TimeSpan.FromDays(
				extra.GetOrDefault(AdminExtraConfigKeys.SessionExpireDaysWithoutRememberLogin, 1));
			AvatarWidth = extra.GetOrDefault(AdminExtraConfigKeys.AvatarWidth, 150);
			AvatarHeight = extra.GetOrDefault(AdminExtraConfigKeys.AvatarHeight, 150);
			AvatarImageQuality = 90;
		}

		/// <summary>
		/// 注册用户
		/// 注册失败时会抛出例外
		/// </summary>
		public virtual void Reg(
			string username, string password, Action<User> update = null) {
			var user = new User() {
				Type = NormalUserType.ConstType,
				Username = username
			};
			user.SetPassword(password);
			Save(ref user);
		}

		/// <summary>
		/// 根据用户名查找用户
		/// 找不到时返回null
		/// </summary>
		public virtual User FindUser(string username) {
			var uow = UnitOfWork;
			var handlers = Application.Ioc.ResolveMany<IUserLoginHandler>();
			User user = null;
			using (uow.Scope()) {
				// 通过处理器查找用户
				foreach (var handler in handlers) {
					user = handler.FindUser(username);
					if (user != null) {
						return user;
					}
				}
				// 通过用户名查找用户
				// 默认过滤器会过滤已删除的用户
				user = Get(u => u.Username == username);
			}
			return user;
		}

		/// <summary>
		/// 登陆用户
		/// 登陆失败时会抛出例外
		/// </summary>
		public virtual void Login(string username, string password, bool rememberLogin) {
			// 用户不存在或密码错误时抛出例外
			var user = FindUser(username);
			if (user == null || !user.CheckPassword(password)) {
				throw new ForbiddenException(new T("Incorrect username or password"));
			}
			// 以指定用户登录
			LoginWithUser(user, rememberLogin);
		}

		/// <summary>
		/// 以指定用户登录
		/// 跳过密码等检查
		/// </summary>
		public virtual void LoginWithUser(User user, bool rememberLogin) {
			// 获取回调
			var handlers = Application.Ioc.ResolveMany<IUserLoginHandler>().ToList();
			// 登陆前的处理
			handlers.ForEach(c => c.BeforeLogin(user));
			// 设置会话
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession(false);
			var session = sessionManager.GetSession();
			session.ReGenerateId();
			session.ReleatedId = user.Id;
			session.RememberLogin = rememberLogin;
			session.SetExpiresAtLeast(session.RememberLogin ?
				SessionExpireDaysWithRememebrLogin : SessionExpireDaysWithoutRememberLogin);
			sessionManager.SaveSession();
			// 登陆后的处理
			handlers.ForEach(c => c.AfterLogin(user));
		}

		/// <summary>
		/// 退出登录
		/// </summary>
		public virtual void Logout() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession(true);
		}

		/// <summary>
		/// 获取登录后应该跳转到的url
		/// </summary>
		/// <returns></returns>
		public virtual string GetUrlRedirectAfterLogin() {
			var request = HttpManager.CurrentContext.Request;
			var referer = request.GetReferer();
			// 来源于同一站点时，跳转到来源页面
			if (referer != null && referer.Authority == request.Host &&
				!referer.AbsolutePath.Contains("/logout") &&
				!referer.AbsolutePath.Contains("/login")) {
				return referer.PathAndQuery;
			}
			// 默认跳转到首页
			return BaseFilters.Url("/");
		}

		/// <summary>
		/// 获取用户头像的网页图片路径，不存在时返回默认图片路径
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual string GetAvatarWebPath(Guid userId) {
			if (!GetAvatarStorageFile(userId).Exists) {
				// 没有自定义头像时使用默认头像
				return "/static/common.admin.images/default-avatar.jpg";
			}
			return string.Format("/static/common.admin.images/avatar_{0}.jpg", userId);
		}

		/// <summary>
		/// 获取用户头像的储存路径，文件不一定存在
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual IFileEntry GetAvatarStorageFile(Guid userId) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			return fileStorage.GetStorageFile(
				"static", "common.admin.images", string.Format("avatar_{0}.jpg", userId));
		}

		/// <summary>
		/// 保存头像，返回是否成功和错误信息
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="imageStream">图片数据流</param>
		public virtual void SaveAvatar(Guid userId, Stream imageStream) {
			if (imageStream == null) {
				throw new BadRequestException(new T("Please select avatar file"));
			}
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new BadRequestException(new T("Parse uploaded image failed"));
			}
			using (image) {
				var fileEntry = GetAvatarStorageFile(userId);
				using (var newImage = image.Resize(
					AvatarWidth, AvatarHeight, ImageResizeMode.Padding, Color.White)) {
					using (var stream = fileEntry.OpenWrite()) {
						newImage.SaveAuto(stream, ".jpg", AvatarImageQuality);
					}
				}
			}
		}

		/// <summary>
		/// 删除头像
		/// </summary>
		/// <param name="userId">用户Id</param>
		public void DeleteAvatar(Guid userId) {
			GetAvatarStorageFile(userId).Delete();
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="oldPassword">原密码</param>
		/// <param name="newPassword">新密码</param>
		public void ChangePassword(Guid userId, string oldPassword, string newPassword) {
			using (UnitOfWork.Scope()) {
				var user = Get(userId);
				if (user == null) {
					throw new ForbiddenException(new T("User not found"));
				} else if (!user.CheckPassword(oldPassword)) {
					throw new ForbiddenException(new T("Incorrect old password"));
				}
				Save(ref user, u => u.SetPassword(newPassword));
			}
		}
	}
}
