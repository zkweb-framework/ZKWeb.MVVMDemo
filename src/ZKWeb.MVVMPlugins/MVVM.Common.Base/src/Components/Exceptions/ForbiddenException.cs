using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions {
	/// <summary>
	/// 无权访问的错误
	/// </summary>
	public class ForbiddenException : HttpException {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="message">错误消息</param>
		public ForbiddenException(string message) : base(403, message) { }
	}
}
