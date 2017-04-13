using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Entities;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions {
	/// <summary>
	/// 会话的扩展函数
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// 用于在当前Http请求当前会话对应的用户
		/// </summary>
		public const string SessionUserContextKey = "ZKWeb.SessionUser";

		/// <summary>
		/// 获取会话对应的用户
		/// </summary>
		public static User GetUser(this Session session) {
			// 会话没有对应用户
			if (session.UserId == null) {
				return null;
			}
			// 从Http上下文中获取，确保保存时的会话和获取时的会话是同一个
			if (HttpManager.CurrentContextExists) {
				var context = HttpManager.CurrentContext;
				var pair = context.GetData<Tuple<Session, User>>(SessionUserContextKey);
				if (pair != null && pair.Item1 == session) {
					return pair.Item2;
				}
			}
			// 从服务获取
			var service = Application.Ioc.Resolve<IDomainService<User, Guid>>();
			var user = service.Get(session.UserId.Value);
			if (HttpManager.CurrentContextExists) {
				var context = HttpManager.CurrentContext;
				context.PutData(SessionUserContextKey, Tuple.Create(session, user));
			}
			return user;
		}
	}
}
