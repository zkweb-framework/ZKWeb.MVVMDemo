using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.UserTypes;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// 管理员管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class AdminManager : DomainServiceBase {
		/// <summary>
		/// 判断系统是否没有管理员
		/// </summary>
		public virtual bool HaveNoAdmin() {
			var adminTypes = Application.Ioc.ResolveMany<IUserType>()
				.Where(t => t is IAmAdmin).Select(t => t.Type).ToList();
			var userService = Application.Ioc.Resolve<IDomainService<User, Guid>>();
			return userService.Count(u => adminTypes.Contains(u.Type)) == 0;
		}

		/// <summary>
		/// 登陆管理员
		/// 登陆失败时会抛出例外
		/// </summary>
		public virtual void Login(string username, string password, bool rememberLogin) {
			var userManager = Application.Ioc.Resolve<UserManager>();
			var user = userManager.FindUser(username);
			// 用户不存在或密码错误时抛出例外
			if (user == null || !user.CheckPassword(password)) {
				throw new ForbiddenException(new T("Incorrect username or password"));
			}
			// 当前没有任何管理员时，把这个用户设置为超级管理员
			var uow = UnitOfWork;
			using (uow.Scope()) {
				uow.Context.BeginTransaction();
				if (HaveNoAdmin()) {
					var userService = Application.Ioc.Resolve<IDomainService<User, Guid>>();
					userService.Save(ref user, u => u.Type = SuperAdminUserType.ConstType);
				}
				uow.Context.FinishTransaction();
			}
			// 只允许管理员或合作伙伴登陆到后台
			if (!(user.GetUserType() is ICanUseAdminPanel)) {
				throw new ForbiddenException(new T("Sorry, You have no privileges to use admin panel."));
			}
			// 以指定用户登录
			userManager.LoginWithUser(user, rememberLogin);
		}

		/// <summary>
		/// 获取登录管理员时的警告信息
		/// </summary>
		/// <returns></returns>
		public virtual string GetLoginWarning() {
			// 当前没有任何管理员，第一次登录的用户将成为超级管理员
			if (HaveNoAdmin()) {
				return new T("Website has no admin yet, the first login user will become super admin.");
			}
			// 警告当前登录的用户非管理员
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user != null) {
				return new T("You have already logged in, continue will replace the logged in user.");
			}
			return null;
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
			// 默认跳转到后台首页
			return BaseFilters.Url("/admin");
		}
	}
}
