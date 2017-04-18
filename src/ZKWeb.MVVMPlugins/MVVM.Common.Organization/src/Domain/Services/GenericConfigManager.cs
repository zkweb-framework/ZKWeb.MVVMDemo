using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using ZKWeb.Cache;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services {
	/// <summary>
	/// 通用配置管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericConfigManager :
		DomainServiceBase<GenericConfig, Guid>, ICacheCleaner {
		/// <summary>
		/// 配置属性的缓存
		/// </summary>
		protected ConcurrentDictionary<Type, GenericConfigAttribute> AttributeCache { get; set; }
		/// <summary>
		/// 配置值的缓存
		/// </summary>
		protected IKeyValueCache<Type, object> ConfigValueCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericConfigManager() {
			var cacheFactory = ZKWeb.Application.Ioc.Resolve<ICacheFactory>();
			AttributeCache = new ConcurrentDictionary<Type, GenericConfigAttribute>();
			ConfigValueCache = cacheFactory.CreateCache<Type, object>();
		}

		/// <summary>
		/// 获取类型标记的配置属性
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns></returns>
		protected virtual GenericConfigAttribute GetConfigAttribute<T>() {
			var attribute = AttributeCache.GetOrAdd(
				typeof(T), t => t.GetTypeInfo().GetAttribute<GenericConfigAttribute>());
			if (attribute == null) {
				throw new ArgumentException(string.Format(
					"type {0} not contains GenericConfigAttribute", typeof(T).Name));
			}
			return attribute;
		}

		/// <summary>
		/// 储存配置值
		/// </summary>
		public virtual void PutData<T>(T value) where T : class, new() {
			var attribute = GetConfigAttribute<T>();
			var key = attribute.Key;
			// 保存到数据库
			var json = JsonConvert.SerializeObject(value);
			var uow = UnitOfWork;
			using (uow.Scope()) {
				uow.Context.BeginTransaction();
				var config = Repository.Get(c => c.Key == key);
				config = config ?? new GenericConfig() { Key = key };
				Repository.Save(ref config, c => c.Value = json);
				uow.Context.FinishTransaction();
			}
			// 保存到缓存
			if (attribute.CacheTime > 0) {
				ConfigValueCache.Put(typeof(T), value, TimeSpan.FromSeconds(attribute.CacheTime));
			}
		}

		/// <summary>
		/// 获取配置值，没有找到时返回新的值
		/// </summary>
		/// <returns></returns>
		public virtual T GetData<T>() where T : class, new() {
			// 从缓存获取
			var value = ConfigValueCache.GetOrDefault(typeof(T)) as T;
			if (value != null) {
				return value;
			}
			// 从数据库获取
			var attribute = GetConfigAttribute<T>();
			var key = attribute.Key;
			var config = Get(c => c.Key == key);
			if (config != null) {
				value = JsonConvert.DeserializeObject<T>(config.Value);
			}
			// 允许缓存时设置到缓存
			value = value ?? new T();
			if (attribute.CacheTime > 0) {
				ConfigValueCache.Put(typeof(T), value, TimeSpan.FromSeconds(attribute.CacheTime));
			}
			return value;
		}

		/// <summary>
		/// 删除配置值
		/// </summary>
		public virtual void RemoveData<T>() {
			var attribute = GetConfigAttribute<T>();
			var key = attribute.Key;
			// 从缓存删除
			ConfigValueCache.Remove(typeof(T));
			// 从数据库删除
			using (UnitOfWork.Scope()) {
				Repository.BatchDelete(c => c.Key == key);
			}
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			ConfigValueCache.Clear();
		}
	}
}
