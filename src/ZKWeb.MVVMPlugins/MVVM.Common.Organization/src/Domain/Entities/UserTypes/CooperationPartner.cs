using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.UserTypes {
	/// <summary>
	/// 用户类型: 合作伙伴
	/// </summary>
	[ExportMany]
	public class CooperationPartner : IAmCooperationPartner {
		public const string ConstType = "CooperationPartner";
		public string Type { get { return ConstType; } }
	}
}
