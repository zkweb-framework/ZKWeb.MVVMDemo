using System;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities {
	/// <summary>
	/// 通用配置
	/// </summary>
	[ExportMany]
	public class GenericConfig :
		IEntity<string>, IHaveCreateTime, IHaveUpdateTime,
		IEntityMappingProvider<GenericConfig> {
		/// <summary>
		/// 主键，配置名称
		/// </summary>
		public virtual string Id { get; set; }
		/// <summary>
		/// 配置值（json）
		/// </summary>
		public virtual string Value { get; set; }
		/// <summary>
		/// 创建时间
		/// 20170225: 之前忘记添加这个字段，会导致旧记录显示创建时间是初始值
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }

		/// <summary>
		/// 配置的数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<GenericConfig> builder) {
			builder.Id(c => c.Id);
			builder.Map(c => c.Value);
			builder.Map(c => c.CreateTime);
			builder.Map(c => c.UpdateTime);
		}
	}
}
