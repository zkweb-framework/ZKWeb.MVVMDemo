using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Filters;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Services;
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
		/// 超级管理员的名称
		/// </summary>
		protected const string SuperAdminName = "admin";
		/// <summary>
		/// 超级管理员的密码
		/// </summary>
		protected const string SuperAdminPassword = "123456";
		/// <summary>
		/// 生成超级管理员时使用的锁
		/// </summary>
		protected object SuperAdminLock { get; set; } = new object();

		/// <summary>
		/// 获取当前的超级管理员
		/// 有多个时也只返回一个
		/// </summary>
		/// <returns></returns>
		protected virtual User GetSuperAdmin() {
			var superAdminTypes = ZKWeb.Application.Ioc.ResolveMany<IUserType>()
				.Where(t => t is IAmSuperAdmin).Select(t => t.Type).ToList();
			var userManager = ZKWeb.Application.Ioc.Resolve<UserManager>();
			return userManager.Get(u => superAdminTypes.Contains(u.Type));
		}

		/// <summary>
		/// 确保超级管理员的存在
		/// 不存在时创建
		/// 返回原有或创建后的超级管理员
		/// </summary>
		/// <returns></returns>
		public virtual User EnsureSuperAdmin() {
			using (UnitOfWork.Scope())
			using (UnitOfWork.DisableFilter(typeof(OwnerTenantFilter))) {
				var admin = GetSuperAdmin();
				if (admin != null) {
					return admin;
				}
				lock (SuperAdminLock) {
					var tenantManager = ZKWeb.Application.Ioc.Resolve<TenantManager>();
					var masterTenant = tenantManager.EnsureMasterTenant();
					var userManager = ZKWeb.Application.Ioc.Resolve<UserManager>();
					using (UnitOfWork.Scope()) {
						UnitOfWork.Context.BeginTransaction();
						admin = GetSuperAdmin();
						if (admin != null) {
							return admin;
						}
						admin = new User() {
							Type = SuperAdminUserType.ConstType,
							Username = SuperAdminName,
							OwnerTenant = tenantManager.Get(masterTenant.Id),
						};
						admin.SetPassword(SuperAdminPassword);
						userManager.Save(ref admin);
						UnitOfWork.Context.FinishTransaction();
					}
					return admin;
				}
			}
		}

		/// <summary>
		/// 登陆管理员
		/// 登陆失败时会抛出例外
		/// </summary>
		public virtual void Login(string username, string password, bool rememberLogin) {
			var userManager = ZKWeb.Application.Ioc.Resolve<UserManager>();
			var user = userManager.FindUser(username);
			// 用户不存在或密码错误时抛出例外
			if (user == null || !user.CheckPassword(password)) {
				throw new ForbiddenException(new T("Incorrect username or password"));
			}
			// 只允许管理员或合作伙伴登陆到后台
			if (!(user.GetUserType() is ICanUseAdminPanel)) {
				throw new ForbiddenException(new T("Sorry, You have no privileges to use admin panel."));
			}
			// 以指定用户登录
			userManager.LoginWithUser(user, rememberLogin);
		}
	}
}
