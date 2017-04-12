using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// 脚本路径的配置
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ScriptPathConfig {
		/// <summary>
		/// 生成模块的文件夹路径
		/// </summary>
		public string GenerateModuleDirectory { get; set; }
		/// <summary>
		/// 保存数据传输脚本的文件夹名称
		/// </summary>
		public string DtosDirectoryName { get; set; }
		/// <summary>
		/// 保存应用服务脚本的文件夹名称
		/// </summary>
		public string ServicesDirectoryName { get; set; }
		/// <summary>
		/// 保存翻译脚本的文件夹名称
		/// </summary>
		public string TranslationsDirectoryName { get; set; }
		/// <summary>
		/// 生成模块的文件名
		/// </summary>
		public string GeneratedModuleFilename { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ScriptPathConfig() {
			var pluginManager = ZKWeb.Application.Ioc.Resolve<PluginManager>();
			var websitePlugin = pluginManager.Plugins
				.FirstOrDefault(p => p.Directory.EndsWith("MVVM.Angular.Website"));
			if (websitePlugin == null) {
				throw new DirectoryNotFoundException(
					"Plugin 'MVVM.Angular.Website' not found, please check your plugin list");
			}
			GenerateModuleDirectory = PathUtils.SecureCombine(
				websitePlugin.Directory,
				"static", "src", "modules", "generated_module");
			DtosDirectoryName = "dtos";
			ServicesDirectoryName = "services";
			TranslationsDirectoryName = "translations";
			GeneratedModuleFilename = "generated.module.ts";
		}

		/// <summary>
		/// 转换文件名称
		/// 例
		/// "ExampleService" => "example-service"
		/// "MyHTTPService" => "my-http-service"
		/// "zh-CN" => "zh-cn"
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		public virtual string NormalizeFilename(string filename) {
			var result = new StringBuilder() { Capacity = filename.Length + 8 };
			bool lastCharIsLower = false;
			foreach (var c in filename) {
				if (char.IsUpper(c)) {
					if (lastCharIsLower) {
						result.Append('-');
						lastCharIsLower = false;
					}
					result.Append(char.ToLower(c));
				} else {
					lastCharIsLower = char.IsLower(c);
					result.Append(c);
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// 转换类名称
		/// 例
		/// "Translation_zh-CN" => "Translation_zh_CN"
		/// </summary>
		/// <param name="className">类名</param>
		/// <returns></returns>
		public virtual string NormalizeClassName(string className) {
			return className.Replace('-', '_');
		}

		/// <summary>
		/// 获取类型的类名称，支持反省
		/// </summary>
		/// <param name="type">类型</param>
		/// <returns></returns>
		public virtual string NormalizeClassName(Type type) {
			var typeInfo = type.GetTypeInfo();
			if (typeInfo.IsGenericType) {
				var typename = type.Name.Split('`')[0] + "_";
				foreach (var genericType in typeInfo.GetGenericArguments()) {
					typename += NormalizeClassName(genericType);
				}
				return typename;
			}
			return type.Name;
		}
	}
}
