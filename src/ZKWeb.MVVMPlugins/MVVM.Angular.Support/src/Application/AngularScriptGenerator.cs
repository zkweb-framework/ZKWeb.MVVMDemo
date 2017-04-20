using System;
using System.Collections.Generic;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Application {
	/// <summary>
	/// 负责处理AngularJS的服务脚本
	/// </summary>
	[ExportMany, SingletonReuse]
	public class AngularScriptGenerator {
		/// <summary>
		/// 已生成脚本的数据传输对象类型
		/// </summary>
		protected ISet<Type> GeneratedDtoTypes { get; set; }
		/// <summary>
		/// 已生成脚本的应用服务
		/// </summary>
		protected ISet<IApplicationService> GeneratedApplicationServices { get; set; }
		/// <summary>
		/// 已生成脚本的语言
		/// </summary>
		protected ISet<string> GeneratedTranslationLanguages { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AngularScriptGenerator() {
			GeneratedDtoTypes = new HashSet<Type>();
			GeneratedApplicationServices = new HashSet<IApplicationService>();
			GeneratedTranslationLanguages = new HashSet<string>();
		}

		/// <summary>
		/// 根据数据传输对象生成脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateDtoScript(Type type) {
			// 递归生成所有关联的对象传输对象
			// 例如 class A { B b; } 的时候可以生成A和B的脚本	
			var generator = ZKWeb.Application.Ioc.Resolve<DtoScriptGenerator>();
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var discoveredTypes = new List<Type>() { type };
			while (discoveredTypes.Count > 0) {
				// 出栈
				var discoveredType = discoveredTypes[discoveredTypes.Count - 1];
				discoveredTypes.RemoveAt(discoveredTypes.Count - 1);
				// 如果不需要生成或已经生成过则跳过
				if (!generator.ShouldGenerate(discoveredType) ||
					GeneratedDtoTypes.Contains(discoveredType)) {
					continue;
				}
				// 生成此类型的脚本
				var script = generator.GenerateScript(discoveredType, discoveredTypes);
				var filename = pathConfig.NormalizeFilename(
					pathConfig.NormalizeClassName(discoveredType)) + ".ts";
				var path = PathUtils.SecureCombine(
					pathConfig.GenerateModuleDirectory,
					pathConfig.DtosDirectoryName,
					filename);
				PathUtils.EnsureParentDirectory(path);
				File.WriteAllText(path, script);
				// 添加到已生成的集合
				GeneratedDtoTypes.Add(discoveredType);
			}
		}

		/// <summary>
		/// 根据应用服务生成脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateServiceScript(IApplicationService service) {
			// 如果已生成过则跳过
			if (GeneratedApplicationServices.Contains(service)) {
				return;
			}
			var generator = ZKWeb.Application.Ioc.Resolve<ServiceScriptGenerator>();
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			// 生成此应用服务的脚本
			var script = generator.GenerateScript(service);
			var filename = pathConfig.NormalizeFilename(
				pathConfig.NormalizeClassName(service.GetType())) + ".ts";
			var path = PathUtils.SecureCombine(
				pathConfig.GenerateModuleDirectory,
				pathConfig.ServicesDirectoryName,
				filename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
			// 添加到已生成的集合
			GeneratedApplicationServices.Add(service);
		}

		/// <summary>
		/// 根据翻译的语言生成脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateTranslationScript(string language) {
			// 如果已生成过则跳过
			if (GeneratedTranslationLanguages.Contains(language)) {
				return;
			}
			var generator = ZKWeb.Application.Ioc.Resolve<TranslationScriptGenerator>();
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			// 生成此语言的脚本
			var script = generator.GenerateScript(language);
			var filename = pathConfig.NormalizeFilename(language) + ".ts";
			var path = PathUtils.SecureCombine(
				pathConfig.GenerateModuleDirectory,
				pathConfig.TranslationsDirectoryName,
				filename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
			// 添加到已生成的集合
			GeneratedTranslationLanguages.Add(language);
		}

		/// <summary>
		/// 根据之前翻译的语言生成索引脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateTranslationIndexScript() {
			var generator = ZKWeb.Application.Ioc.Resolve<TranslationScriptGenerator>();
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var script = generator.GenerateIndexScript(GeneratedTranslationLanguages);
			var filename = "index.ts";
			var path = PathUtils.SecureCombine(
				pathConfig.GenerateModuleDirectory,
				pathConfig.TranslationsDirectoryName,
				filename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
		}

		/// <summary>
		/// 生成用户类型和权限列表脚本，并写入到文件
		/// </summary>
		protected virtual void GeneratePrivilegesScript() {
			var generator = ZKWeb.Application.Ioc.Resolve<PrivilegeScriptGenerator>();
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var userTypesScript = generator.GenerateUserTypesScript();
			var privilegesScript = generator.GeneratePrivilegesScript();
			var userTypesPath = PathUtils.SecureCombine(
				pathConfig.GenerateModuleDirectory,
				pathConfig.PrivilegesDirectoryName,
				"user-types.ts");
			var privilegesPath = PathUtils.SecureCombine(
				pathConfig.GenerateModuleDirectory,
				pathConfig.PrivilegesDirectoryName,
				"privileges.ts");
			PathUtils.EnsureParentDirectory(userTypesPath);
			File.WriteAllText(userTypesPath, userTypesScript);
			File.WriteAllText(privilegesPath, privilegesScript);
		}

		/// <summary>
		/// 根据之前生成的脚本生成模块脚本，并写入到文件
		/// </summary>
		protected virtual void GenerateModuleScript() {
			var generator = ZKWeb.Application.Ioc.Resolve<ModuleScriptGenerator>();
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var script = generator.GenerateScript(GeneratedApplicationServices);
			var path = PathUtils.SecureCombine(
				pathConfig.GenerateModuleDirectory,
				pathConfig.GeneratedModuleFilename);
			PathUtils.EnsureParentDirectory(path);
			File.WriteAllText(path, script);
		}

		/// <summary>
		/// 生成所有脚本
		/// </summary>
		public virtual void GenerateAll() {
			// 清空上次生成的集合
			GeneratedDtoTypes.Clear();
			GeneratedApplicationServices.Clear();
			GeneratedTranslationLanguages.Clear();
			// 生成应用服务的脚本
			var applicationServices = ZKWeb.Application.Ioc.ResolveMany<ApplicationServiceBase>();
			foreach (var applicationService in applicationServices) {
				// 生成数据传输对象的脚本
				foreach (var method in applicationService.GetApiMethods()) {
					// 生成返回类型
					GenerateDtoScript(method.ReturnType);
					// 生成参数类型
					foreach (var parameter in method.Parameters) {
						GenerateDtoScript(parameter.Type);
					}
				}
				// 生成应用服务的脚本
				GenerateServiceScript(applicationService);
			}
			// 生成翻译的脚本
			var languages = ZKWeb.Application.Ioc.ResolveMany<ILanguage>();
			foreach (var language in languages) {
				GenerateTranslationScript(language.Name);
			}
			// 生成翻译的索引脚本
			GenerateTranslationIndexScript();
			// 生成用户类型和权限列表的脚本
			GeneratePrivilegesScript();
			// 生成模块的脚本
			GenerateModuleScript();
		}
	}
}
