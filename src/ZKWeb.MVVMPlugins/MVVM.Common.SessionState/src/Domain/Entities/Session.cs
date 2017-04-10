using System;
using ZKWebStandard.Ioc;
using ZKWeb.Database;
using System.Collections.Generic;
using Newtonsoft.Json;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Entities {
	/// <summary>
	/// 会话
	/// </summary>
	[ExportMany]
	public class Session :
		IEntity<Guid>,
		IEntityMappingProvider<Session> {
		/// <summary>
		/// 会话Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 关联的用户Id
		/// 未登录时等于null
		/// </summary>
		public virtual Guid? UserId { get; set; }
		/// <summary>
		/// 关联的租户Id
		/// 未登录时等于null
		/// </summary>
		public virtual Guid? TenantId { get; set; }
		/// <summary>
		/// 会话数据的Json
		/// </summary>
		public virtual string ItemsJson { get; set; }
		private IDictionary<string, object> _items;
		/// <summary>
		/// 会话对应的Ip地址
		/// </summary>
		public virtual string IpAddress { get; set; }
		/// <summary>
		/// 是否记住登录
		/// </summary>
		public virtual bool RememberLogin { get; set; }
		/// <summary>
		/// 过期时间
		/// </summary>
		public virtual DateTime Expires { get; set; }

		/// <summary>
		/// 设置会话数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object this[string key] {
			get {
				if (_items == null) {
					_items = string.IsNullOrEmpty(ItemsJson) ?
						new Dictionary<string, object>() :
						JsonConvert.DeserializeObject<IDictionary<string, object>>(ItemsJson);
				}
				return _items.GetOrDefault(key);
			}
			set {
				if (_items == null) {
					_items = string.IsNullOrEmpty(ItemsJson) ?
						new Dictionary<string, object>() :
						JsonConvert.DeserializeObject<IDictionary<string, object>>(ItemsJson);
				}
				_items[key] = value;
				ItemsJson = JsonConvert.SerializeObject(_items);
			}
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<Session> builder) {
			builder.Id(s => s.Id);
			builder.Map(s => s.UserId, new EntityMappingOptions() { Index = "Idx_userId" });
			builder.Map(s => s.TenantId, new EntityMappingOptions() { Index = "Idx_TenantId" });
			builder.Map(s => s.ItemsJson);
			builder.Map(s => s.IpAddress);
			builder.Map(s => s.RememberLogin);
			builder.Map(s => s.Expires, new EntityMappingOptions() { Index = "Idx_Expires" });
		}
	}
}
