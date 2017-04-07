using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Structs;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases {
	/// <summary>
	/// 应用服务的基础类
	/// 继承这个类以后，所有公有的函数都会运行在工作单元中
	/// 并且公有函数会对应一个Api地址，格式是"/api/类名/函数名"
	/// </summary>
	public class ApplicationServiceBase :
		IController, IApplicationService, IWebsiteStartHandler {
		/// <summary>
		/// 当前的Http上下文
		/// </summary>
		protected virtual IHttpContext Context => HttpManager.CurrentContext;
		/// <summary>
		/// 当前的Http请求
		/// </summary>
		protected virtual IHttpRequest Request => Context.Request;
		/// <summary>
		/// 当前的Http回应
		/// </summary>
		protected virtual IHttpResponse Response => Context.Response;
		/// <summary>
		/// 当前使用的工作单元
		/// </summary>
		protected virtual IUnitOfWork UnitOfWork => ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
		/// <summary>
		/// 基础地址
		/// </summary>
		protected virtual string UrlBase => $"/api/{GetType().Name}";

		/// <summary>
		/// 包装Api函数
		/// </summary>
		protected virtual Func<IActionResult> WrapApiMethod(
			ApplicationServiceApiMethodInfo methodInfo) {
			var attribute = methodInfo.Attributes
				.OfType<UnitOfWorkAttribute>().FirstOrDefault() ?? new UnitOfWorkAttribute();
			if (attribute.IsDisabled) {
				// 不使用工作单元
				return methodInfo.Action;
			}
			return new Func<IActionResult>(() => {
				var uow = UnitOfWork;
				// 使用工作单元
				using (uow.Scope()) {
					// 并且使用事务
					if (attribute.IsTransactional) {
						uow.Context.BeginTransaction(attribute.IsolationLevel);
					}
					var result = methodInfo.Action();
					if (attribute.IsTransactional) {
						uow.Context.FinishTransaction();
					}
					return result;
				}
			});
		}

		/// <summary>
		/// 获取Api函数列表
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<ApplicationServiceApiMethodInfo> GetApiMethods() {
			var typeInfo = GetType().GetTypeInfo();
			var methods = typeInfo.GetMethods(
				BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			foreach (var method in methods) {
				if (method.IsSpecialName)
					continue;
				if (method.ReturnType == typeof(void))
					continue;
				var info = new ApplicationServiceApiMethodInfo(
					method.ReturnType,
					method.Name,
					$"{UrlBase}/{method.Name}",
					method.FastGetCustomAttributes(typeof(Attribute), true).OfType<Attribute>(),
					method.GetParameters().Select(p => new ApplicationServiceApiParameterInfo(
						p.ParameterType,
						p.Name,
						p.GetCustomAttributes(typeof(Attribute), true).OfType<Attribute>(),
						p)),
					this.BuildActionDelegate(method),
					method);
				yield return info;
			}
		}

		/// <summary>
		/// 网站启动时注册所有Api函数
		/// </summary>
		public void OnWebsiteStart() {
			var typeInfo = GetType().GetTypeInfo();
			var methods = typeInfo.GetMethods(
				BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			var controllerManager = ZKWeb.Application.Ioc.Resolve<ControllerManager>();
			foreach (var methodInfo in GetApiMethods()) {
				var url = methodInfo.Url;
				var action = WrapApiMethod(methodInfo);
				controllerManager.RegisterAction(url, HttpMethods.POST, action);
			}
		}
	}
}
