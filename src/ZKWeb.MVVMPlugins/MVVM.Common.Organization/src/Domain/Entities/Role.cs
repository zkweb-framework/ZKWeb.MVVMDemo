using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities {
	/// <summary>
	/// 角色
	/// </summary>
	[ExportMany]
	public class Role :
		IEntity<Guid>,
		IHaveCreateTime,
		IHaveUpdateTime,
		IHaveDeleted,
		IHaveOwnerTenant,
		IEntityMappingProvider<Role> {
		/// <summary>
		/// 角色Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 权限列表
		/// </summary>
		public virtual string PrivilegesJson { get; set; }
		/// <summary>
		/// 所属的租户
		/// </summary>
		public virtual Tenant OwnerTenant { get; set; }
		public virtual Guid OwnerTenantId { get; set; }
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
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 关联的用户
		/// </summary>
		public virtual IList<UserToRole> Users { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Role() {
			Users = new List<UserToRole>();
		}

		/// <summary>
		/// 显示角色名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Name;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<Role> builder) {
			builder.Id(r => r.Id);
			builder.Map(r => r.Name);
			builder.Map(r => r.PrivilegesJson);
			builder.Map(r => r.OwnerTenant, new EntityMappingOptions() { Nullable = false });
			builder.Map(r => r.CreateTime);
			builder.Map(r => r.UpdateTime);
			builder.Map(r => r.Remark);
			builder.Map(r => r.Deleted);
			builder.HasMany(r => r.Users);
		}
	}
}
