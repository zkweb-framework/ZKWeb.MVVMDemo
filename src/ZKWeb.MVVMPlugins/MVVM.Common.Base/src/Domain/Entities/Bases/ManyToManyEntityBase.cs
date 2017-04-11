using System;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Bases {
	/// <summary>
	/// 多对多实体的基类
	/// EFCore需要一个单独的实体类来实现多对多
	/// </summary>
	/// <typeparam name="TFrom">来源实体类型</typeparam>
	/// <typeparam name="TTo">目标实体类型</typeparam>
	/// <typeparam name="TSelf">继承这个类的实体类型</typeparam>
	public class ManyToManyEntityBase<TFrom, TTo, TSelf> :
		IEntity<Guid>,
		IEntityMappingProvider<TSelf>
		where TFrom : class, IEntity
		where TTo : class, IEntity
		where TSelf : ManyToManyEntityBase<TFrom, TTo, TSelf> {
		/// <summary>
		/// 主键
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 来源实体
		/// </summary>
		public virtual TFrom From { get; set; }
		/// <summary>
		/// 目标实体
		/// </summary>
		public virtual TTo To { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// EF指定不支持多对多的级联删除(may cause cycles or multiple cascade paths)
		/// 有需要请手动删除数据
		/// </summary>
		/// <param name="builder"></param>
		public void Configure(IEntityMappingBuilder<TSelf> builder) {
			builder.Id(e => e.Id);
			builder.References(e => e.From,
				new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
			builder.References(e => e.To,
				new EntityMappingOptions() { Nullable = false, CascadeDelete = false });
		}
	}
}
