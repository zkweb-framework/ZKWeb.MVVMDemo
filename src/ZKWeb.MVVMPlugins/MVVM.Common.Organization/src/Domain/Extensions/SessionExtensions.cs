using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions {
	/// <summary>
	/// 会话的扩展函数
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// 当前会话对应的用户
		/// </summary>
		public const string SessionUserKey = "Common.Admin.SessionUser";

		/// <summary>
		/// 获取会话对应的用户
		/// </summary>
		public static User GetUser(this Session session) {
			// 会话没有对应用户
			if (session.ReleatedId == Guid.Empty) {
				return null;
			}
			// 从Http上下文中获取，确保保存时的会话和获取时的会话是同一个
			var context = HttpManager.CurrentContext;
			var pair = context.GetData<Tuple<Session, User>>(SessionUserKey);
			if (pair != null && pair.Item1 == session) {
				return pair.Item2;
			}
			// 从服务获取
			var service = Application.Ioc.Resolve<IDomainService<User, Guid>>();
			var user = service.GetMany(query => {
				var u = query.FirstOrDefault(ux => ux.Id == session.ReleatedId);
				var _ = u?.Roles.SelectMany(role => role.Privileges).ToList(); // 预读数据
				return u;
			});
			context.PutData(SessionUserKey, Tuple.Create(session, user));
			return user;
		}
	}
}
