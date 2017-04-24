using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 用户管理服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("用户管理服务")]
	public class UserManageService : ApplicationServiceBase {
	}
}
