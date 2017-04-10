using System;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Components.SessionIdStore.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Components.SessionIdSources {
	/// <summary>
	/// 使用Http头获取和保存会话Id
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionIdHeaderStore : ISessionIdStore {
		/// <summary>
		/// 客户端发过来的会话Id
		/// </summary>
		public const string SessionHeaderIn = "X-ZKWebSessionId";
		/// <summary>
		/// 发送给客户端的会话Id
		/// </summary>
		public const string SessionHeaderOut = "X-Set-ZKWebSessionId";

		/// <summary>
		/// 获取会话Id
		/// </summary>
		/// <returns></returns>
		public Guid GetSessionId() {
			var context = HttpManager.CurrentContext;
			return context.Request.GetHeader(SessionHeaderIn).ConvertOrDefault<Guid>();
		}

		/// <summary>
		/// 设置会话Id
		/// </summary>
		/// <param name="sessionId">会话Id</param>
		/// <param name="expires">过期时间</param>
		public void SetSessionId(Guid sessionId, DateTime? expires) {
			var context = HttpManager.CurrentContext;
			context.Response.AddHeader(SessionHeaderOut, sessionId.ToString());
		}

		/// <summary>
		/// 删除会话Id
		/// </summary>
		public void RemoveSessionId() {
			var context = HttpManager.CurrentContext;
			context.Response.AddHeader(SessionHeaderOut, "");
		}
	}
}
