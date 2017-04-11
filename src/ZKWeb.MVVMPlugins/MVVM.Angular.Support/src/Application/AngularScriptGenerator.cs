using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWeb.Plugin;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Application {
	/// <summary>
	/// 负责处理AngularJS的服务脚本
	/// </summary>
	[ExportMany]
	public class AngularScriptGenerator : IWebsiteStartHandler {
		/// <summary>
		/// 生成模块的文件夹路径
		/// </summary>
		protected string GenerateModuleDirectory { get; set; }
		/// <summary>
		/// 保存数据传输脚本的文件夹名称
		/// </summary>
		protected string DtosDirectoryName { get; set; }
		/// <summary>
		/// 保存应用服务脚本的文件夹名称
		/// </summary>
		protected string ServicesDirectoryName { get; set; }
		/// <summary>
		/// 生成模块的文件名
		/// </summary>
		protected string GeneratedModuleFilename { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AngularScriptGenerator() {
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
			GeneratedModuleFilename = "generated.module.ts";
		}

		/// <summary>
		/// 转换文件名称
		/// 例
		/// "ExampleService" => "example-service"
		/// "MyHTTPService" => "my-http-service"
		/// </summary>
		protected virtual string NormalizeFilename(string filename) {
			var result = new StringBuilder() { Capacity = filename.Length + 8 };
			bool lastCharIsNotUpper = false;
			foreach (var c in filename) {
				if (char.IsUpper(c)) {
					if (lastCharIsNotUpper) {
						result.Append('-');
						lastCharIsNotUpper = false;
					}
					result.Append(char.ToLower(c));
				} else {
					lastCharIsNotUpper = true;
					result.Append(c);
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// 根据数据传输对象生成脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateDtoScript(Type type) {
			var generator = ZKWeb.Application.Ioc.Resolve<DtoScriptGenerator>();
			if (!generator.ShouldGenerate(type)) {
				return;
			}
			var script = generator.GenerateScript(type);
			var filename = NormalizeFilename(type.Name) + ".ts";
			var path = PathUtils.SecureCombine(
				GenerateModuleDirectory, DtosDirectoryName, filename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
		}

		/// <summary>
		/// 根据应用服务生成脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateServiceScript(IApplicationService service) {
			var generator = ZKWeb.Application.Ioc.Resolve<ServiceScriptGenerator>();
			var script = generator.GenerateScript(service);
			var filename = NormalizeFilename(service.GetType().Name) + ".ts";
			var path = PathUtils.SecureCombine(
				GenerateModuleDirectory, ServicesDirectoryName, filename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
		}

		/// <summary>
		/// 根据之前生成的脚本生成模块脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateModuleScript(
			ISet<Type> dtos,
			ISet<IApplicationService> applicationServices) {
			var script = "";
			var path = PathUtils.SecureCombine(
				GenerateModuleDirectory, GeneratedModuleFilename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
		}

		/// <summary>
		/// 网站启动时生成脚本
		/// </summary>
		public virtual void OnWebsiteStart() {
			var applicationServices = ZKWeb.Application.Ioc.ResolveMany<ApplicationServiceBase>();
			var generatedDtoTypes = new HashSet<Type>();
			var generatedApplicationServices = new HashSet<IApplicationService>();
			foreach (var applicationService in applicationServices) {
				// 生成数据传输对象的脚本
				foreach (var method in applicationService.GetApiMethods()) {
					// 生成返回类型
					if (!generatedDtoTypes.Contains(method.ReturnType)) {
						GenerateDtoScript(method.ReturnType);
						generatedDtoTypes.Add(method.ReturnType);
					}
					// 生成参数类型
					foreach (var parameter in method.Parameters) {
						if (!generatedDtoTypes.Contains(parameter.Type)) {
							GenerateDtoScript(parameter.Type);
							generatedDtoTypes.Add(parameter.Type);
						}
					}
				}
				// 生成应用服务的脚本
				GenerateServiceScript(applicationService);
				generatedApplicationServices.Add(applicationService);
			}
			// 生成模块的脚本
			GenerateModuleScript(generatedDtoTypes, generatedApplicationServices);
		}
	}
}
