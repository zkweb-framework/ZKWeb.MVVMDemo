using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions {
	/// <summary>
	/// 页面或资源无法找到的错误
	/// </summary>
	public class NotFoundException : HttpException {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="message">错误消息</param>
		public NotFoundException(string message) : base(404, message) { }
	}
}
