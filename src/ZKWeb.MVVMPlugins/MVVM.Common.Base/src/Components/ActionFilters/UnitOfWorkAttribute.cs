using System;
using System.Data;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes {
	/// <summary>
	/// 控制应用服务如何使用工作单元，标记在应用服务的函数上
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class UnitOfWorkAttribute : ActionFilterAttribute {
		/// <summary>
		/// 是否禁用工作单元，默认是false
		/// </summary>
		public bool IsDisabled { get; set; }
		/// <summary>
		/// 是否开启事务。默认是false
		/// </summary>
		public bool IsTransactional { get; set; }
		/// <summary>
		/// 事务的隔离等级，默认是null
		/// </summary>
		public IsolationLevel? IsolationLevel { get; set; }

		/// <summary>
		/// 开启工作单元
		/// </summary>
		public override Func<IActionResult> Filter(Func<IActionResult> action) {
			if (IsDisabled) {
				// 不使用工作单元
				return action;
			}
			return new Func<IActionResult>(() => {
				var uow = ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
				// 使用工作单元
				using (uow.Scope()) {
					// 并且使用事务
					if (IsTransactional) {
						uow.Context.BeginTransaction(IsolationLevel);
					}
					var result = action();
					if (IsTransactional) {
						uow.Context.FinishTransaction();
					}
					return result;
				}
			});
		}
	}
}
