using ZKWeb.Localize;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions {
	/// <summary>
	/// 请求参数有误的错误
	/// </summary>
	public class BadRequestException : HttpException {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="message">错误消息</param>
		public BadRequestException(string message) : base(400, new T(message)) { }
	}
}
