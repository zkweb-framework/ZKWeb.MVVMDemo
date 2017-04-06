using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.SessionIdStore.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.SessionIdSources {
	/// <summary>
	/// 使用Cookie获取和保存会话Id
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionIdCookieStore : ISessionIdStore {
		/// <summary>
		/// 储存会话Id到Cookie时使用的键
		/// </summary>
		public const string SessionCookieKey = "ZKWEBSESSID";

		/// <summary>
		/// 获取会话Id
		/// </summary>
		/// <returns></returns>
		public Guid GetSessionId() {
			var context = HttpManager.CurrentContext;
			return context.GetCookie(SessionCookieKey).ConvertOrDefault<Guid>();
		}

		/// <summary>
		/// 设置会话Id
		/// </summary>
		/// <param name="sessionId">会话Id</param>
		/// <param name="expires">过期时间</param>
		public void SetSessionId(Guid sessionId, DateTime? expires) {
			var options = new HttpCookieOptions() { Expires = expires, HttpOnly = true };
			var context = HttpManager.CurrentContext;
			context.PutCookie(SessionCookieKey, sessionId.ToString(), options);
		}

		/// <summary>
		/// 删除会话Id
		/// </summary>
		public void RemoveSessionId() {
			var context = HttpManager.CurrentContext;
			context.RemoveCookie(SessionCookieKey);
		}
	}
}
