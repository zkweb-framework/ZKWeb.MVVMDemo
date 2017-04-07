using System;
using System.Data;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes {
	/// <summary>
	/// 控制应用服务如何使用工作单元，标记在应用服务的函数上
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class UnitOfWorkAttribute : Attribute {
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
	}
}
