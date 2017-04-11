using System;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces {
	/// <summary>
	/// 包含所属租户的接口
	/// </summary>
	public interface IHaveOwnerTenant : IEntity {
		/// <summary>
		/// 所属的租户
		/// </summary>
		Tenant OwnerTenant { get; set; }
		/// <summary>
		/// 所属的租户Id
		/// </summary>
		Guid OwnerTenantId { get; set; }
	}
}
