using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.DatabaseInitializeHandler {
	/// <summary>
	/// 添加表前缀
	/// </summary>
	[ExportMany]
	public class DatabaseInitializeHandler : IDatabaseInitializeHandler {
		public void ConvertTableName(ref string tableName) {
			tableName = "ZKWeb_" + tableName;
		}
	}
}
