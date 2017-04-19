using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services {
	/// <summary>
	/// 用户登录服务
	/// </summary>
	[ExportMany, SingletonReuse, Description("用户登录服务")]
	public class UserLoginService : ApplicationServiceBase {
		/// <summary>
		/// 获取当前的会话信息
		/// </summary>
		/// <returns></returns>
		[Description("获取当前的会话信息")]
		public virtual ActionResponseDto Login(UserLoginRequestDto request) {
			throw new BadRequestException("测试登录失败");
			return ActionResponseDto.CreateSuccess(JsonConvert.SerializeObject(request));
		}
	}
}
