using System;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Extensions {
	/// <summary>
	/// 会话的扩展函数
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// 重新生成Id
		/// </summary>
		public static void ReGenerateId(this Session session) {
			session.Id = GuidUtils.SecureSequentialGuid(DateTime.UtcNow);
		}

		/// <summary>
		/// 设置会话最少在指定的时间后过期
		/// 当前会话的过期时间比指定的时间要晚时不更新当前的过期时间
		/// </summary>
		/// <param name="session">会话</param>
		/// <param name="span">最少在这个时间后过期</param>
		public static void SetExpiresAtLeast(this Session session, TimeSpan span) {
			var expires = DateTime.UtcNow + span;
			if (session.Expires < expires) {
				session.Expires = expires;
				session.ExpiresUpdated = true;
			}
		}
	}
}
