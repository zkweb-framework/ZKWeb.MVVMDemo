using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities {
	/// <summary>
	/// 租户
	/// </summary>
	public class Tenant :
		IEntity<Guid>,
		IHaveCreateTime,
		IHaveUpdateTime,
		IEntityMappingProvider<Tenant> {
		/// <summary>
		/// 租户Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 租户名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 是否主租户
		/// </summary>
		public virtual bool IsMaster { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		/// <param name="builder"></param>
		public void Configure(IEntityMappingBuilder<Tenant> builder) {
			builder.Id(t => t.Id);
			builder.Map(t => t.Name, new EntityMappingOptions() { Unique = true });
			builder.Map(t => t.IsMaster, new EntityMappingOptions() { Index = "Idx_IsMaster" });
			builder.Map(t => t.CreateTime);
			builder.Map(t => t.UpdateTime);
			builder.Map(t => t.Remark);
		}
	}
}
