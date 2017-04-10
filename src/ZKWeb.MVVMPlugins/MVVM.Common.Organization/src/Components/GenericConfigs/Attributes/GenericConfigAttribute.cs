using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.GenericConfigs.Attributes {
	/// <summary>
	/// 通用配置的属性，用于指定所属插件和配置键名
	/// 通过配置管理器管理的配置都应该添加这个属性
	/// 这个属性不继承，可以使用新的继承类指定和原来的类不相同的插件和配置键
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class GenericConfigAttribute : Attribute {
		/// <summary>
		/// 配置键名
		/// 格式应该使用"插件.配置键"
		/// </summary>
		public string Key { get; set; }
		/// <summary>
		/// 缓存时间（秒），默认是0
		/// </summary>
		public int CacheTime { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="key">配置键名</param>
		public GenericConfigAttribute(string key) {
			Key = key;
			CacheTime = 0;
		}
	}
}
