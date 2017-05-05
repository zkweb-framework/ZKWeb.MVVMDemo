using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator
{
    /// <summary>
    /// Angular模块的脚本生成器
    /// </summary>
    [ExportMany]
    public class ModuleScriptGenerator
    {
        /// <summary>
        /// 生成模块脚本
        /// </summary>
        public virtual string GenerateScript(
            ISet<IApplicationService> generatedApplicationServices)
        {
            var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
            var includeBuilder = new StringBuilder();
            var moduleBuilder = new StringBuilder();
            var importedNames = new List<string>();
            includeBuilder.AppendLine("import { NgModule } from '@angular/core';");
            includeBuilder.AppendLine("import { BaseModule } from '../base_module/base.module';");
            // 导入生成的应用服务
            foreach (var service in generatedApplicationServices)
            {
                var importName = service.GetType().Name;
                var importFile = pathConfig.NormalizeFilename(importName);
                includeBuilder.AppendLine(
                    $"import {{ {importName} }} from './{pathConfig.ServicesDirectoryName}/{importFile}';");
                importedNames.Add(importName);
            }
            // 定义模块
            moduleBuilder.AppendLine("@NgModule({");
            moduleBuilder.AppendLine("    imports: [BaseModule],");
            moduleBuilder.AppendLine("    providers: [");
            foreach (var name in importedNames)
            {
                moduleBuilder.Append($"        {name}");
                if (name != importedNames.Last())
                {
                    moduleBuilder.Append(",");
                }
                moduleBuilder.AppendLine();
            }
            moduleBuilder.AppendLine("    ]");
            moduleBuilder.AppendLine("})");
            moduleBuilder.AppendLine("export class GeneratedModule { }");
            includeBuilder.AppendLine();
            return includeBuilder.ToString() + moduleBuilder.ToString();
        }
    }
}
