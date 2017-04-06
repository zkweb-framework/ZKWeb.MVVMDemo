using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.SessionIdStore.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services {
	/// <summary>
	/// 会话管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionManager : DomainServiceBase<Session, Guid> {
		/// <summary>
		/// 用于在当前Http请求保存会话
		/// </summary>
		public const string SessionContextKey = "Common.Base.Session";

		/// <summary>
		/// 获取当前Http请求对应的会话
		/// 当前没有会话时返回新的会话
		/// </summary>
		/// <returns></returns>
		public virtual Session GetSession() {
			// 从Http上下文中获取会话
			// 因为一次请求中可能会调用多次GetSession，应该确保返回同一个对象
			var context = HttpManager.CurrentContext;
			var session = context.GetData<Session>(SessionContextKey, null);
			if (session != null) {
				return session;
			}
			// 从数据库中获取会话
			// 当前没有会话时返回新的会话
			var sessionIdStore = Application.Ioc.Resolve<ISessionIdStore>();
			session = Get(sessionIdStore.GetSessionId());
			if (session == null) {
				session = new Session() {
					IpAddress = context.Request.RemoteIpAddress.ToString(),
					RememberLogin = false,
					Expires = DateTime.UtcNow.AddHours(1)
				};
				session.ReGenerateId();
			}
			context.PutData(SessionContextKey, session);
			return session;
		}

		/// <summary>
		/// 添加或更新当前的会话
		/// 必要时发送会话Id到客户端
		/// </summary>
		public virtual void SaveSession() {
			var context = HttpManager.CurrentContext;
			var session = context.GetData<Session>(SessionContextKey, null);
			if (session == null) {
				throw new NullReferenceException("session is null");
			}
			// 添加或更新到数据库中
			var sessionIdStore = Application.Ioc.Resolve<ISessionIdStore>();
			var oldSessionId = sessionIdStore.GetSessionId();
			Save(ref session, null);
			// 检测到会话Id有变化时删除原会话
			if (oldSessionId != session.Id) {
				BatchDeleteForever(new[] { oldSessionId });
			}
			// 发送会话Id到客户端
			// 已存在且过期时间没有更新时不会重复发送
			if (oldSessionId != session.Id || session.ExpiresUpdated) {
				session.ExpiresUpdated = false;
				DateTime? expires = null;
				if (session.RememberLogin) {
					expires = session.Expires.AddYears(1);
				}
				sessionIdStore.SetSessionId(session.Id, expires);
			}
		}

		/// <summary>
		/// 删除当前会话
		/// </summary>
		/// <param name="removeClient">是否同时删除客户端中的会话Id</param>
		public virtual void RemoveSession(bool removeClient) {
			// 删除Http上下文中的会话
			var context = HttpManager.CurrentContext;
			context.RemoveData(SessionContextKey);
			// 删除数据库中的会话
			var sessionIdStore = Application.Ioc.Resolve<ISessionIdStore>();
			var sessionId = sessionIdStore.GetSessionId();
			if (sessionId != Guid.Empty) {
				BatchDeleteForever(new[] { sessionId });
			}
			// 删除客户端中的会话Id
			if (removeClient) {
				sessionIdStore.RemoveSessionId();
			}
		}

		/// <summary>
		/// 删除所有已过期的会话
		/// 返回删除的会话数量
		/// </summary>
		public virtual long RemoveExpiredSessions() {
			var now = DateTime.UtcNow;
			using (UnitOfWork.Scope()) {
				return Repository.BatchDelete(s => s.Expires < now);
			}
		}
	}
}
