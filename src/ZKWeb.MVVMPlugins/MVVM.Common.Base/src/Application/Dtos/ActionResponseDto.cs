using System.ComponentModel;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos {
	/// <summary>
	/// 通用的回应
	/// </summary>
	[Description("通用的回应")]
	public class ActionResponseDto : IOutputDto {
		[Description("是否成功")]
		public bool Success { get; set; }
		[Description("消息")]
		public string Message { get; set; }
		[Description("附加数据")]
		public object Extra { get; set; }

		public static ActionResponseDto CreateSuccess(string message = null, object extra = null) {
			return new ActionResponseDto() { Success = true, Message = message, Extra = extra };
		}

		public static ActionResponseDto CreateFailed(string message, object extra = null) {
			return new ActionResponseDto() { Success = false, Message = message, Extra = extra };
		}
	}
}
