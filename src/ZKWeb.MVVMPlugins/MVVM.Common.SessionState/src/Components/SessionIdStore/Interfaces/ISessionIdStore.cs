using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Components.SessionIdStore.Interfaces {
	/// <summary>
	/// 会话Id的储存接口
	/// </summary>
	public interface ISessionIdStore {
		/// <summary>
		/// 获取会话Id
		/// </summary>
		/// <returns></returns>
		Guid GetSessionId();

		/// <summary>
		/// 设置会话Id
		/// </summary>
		/// <param name="id">会话Id</param>
		/// <param name="expires">过期时间</param>
		void SetSessionId(Guid id, DateTime? expires);

		/// <summary>
		/// 删除会话Id
		/// </summary>
		void RemoveSessionId();
	}
}
