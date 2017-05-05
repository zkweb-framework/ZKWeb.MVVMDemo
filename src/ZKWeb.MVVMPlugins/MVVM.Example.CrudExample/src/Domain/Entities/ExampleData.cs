using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Domain.Entities {
	/// <summary>
	/// 实例数据
	/// </summary>
	[ExportMany]
	public class ExampleData :
		IEntity<Guid>,
		IHaveCreateTime,
		IHaveUpdateTime,
		IHaveDeleted,
		IHaveOwnerTenant,
		IEntityMappingProvider<ExampleData> {
		/// <summary>
		/// 数据Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public virtual string Description { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 所属租户
		/// </summary>
		public virtual Tenant OwnerTenant { get; set; }
		/// <summary>
		/// 所属租户Id
		/// </summary>
		public virtual Guid OwnerTenantId { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public void Configure(IEntityMappingBuilder<ExampleData> builder) {
			builder.Id(e => e.Id);
			builder.Map(e => e.Name, new EntityMappingOptions() { Index = "Idx_Name" });
			builder.Map(e => e.Description);
			builder.Map(e => e.CreateTime);
			builder.Map(e => e.UpdateTime);
			builder.Map(e => e.Deleted);
			builder.References(e => e.OwnerTenant, new EntityMappingOptions() { Nullable = false });
		}
	}
}
