using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Translates.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator
{
    /// <summary>
    /// 翻译的脚本生成器
    /// </summary>
    [ExportMany]
    public class TranslationScriptGenerator
    {
        /// <summary>
        /// 根据翻译的语言生成脚本
        /// </summary>
        public virtual string GenerateScript(string language)
        {
            var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
            var providers = ZKWeb.Application.Ioc.ResolveMany<DictionaryTranslationProviderBase>();
            var allTranslations = new Dictionary<string, string>();
            foreach (var provider in providers)
            {
                if (!provider.CanTranslate(language))
                {
                    continue;
                }
                foreach (var translate in provider.Translates)
                {
                    allTranslations[translate.Key] = translate.Value;
                }
            }
            var classBuilder = new StringBuilder();
            var className = pathConfig.NormalizeClassName("Translation_" + language);
            var classDescription = new T(language).ToString();
            classBuilder.AppendLine($"/** {classDescription} */");
            classBuilder.AppendLine($"export class {className} {{");
            classBuilder.AppendLine($"    public static language = {JsonConvert.SerializeObject(language)};");
            classBuilder.Append($"    public static translations: {{ [key: string]: string }} = ");
            classBuilder.Append(JsonConvert
                .SerializeObject(allTranslations, Formatting.Indented)
                .Replace("\n  ", "\n\t")
                .Replace("\n", "\n\t"));
            classBuilder.AppendLine(";");
            classBuilder.AppendLine("}");
            return classBuilder.ToString();
        }

        /// <summary>
        /// 根据已翻译的语言生成索引脚本
        /// </summary>
        public virtual string GenerateIndexScript(ISet<string> generatedTranslationLanguages)
        {
            var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
            var includeBuilder = new StringBuilder();
            var classBuilder = new StringBuilder();
            var importedLanguages = new List<string>();
            // 导入生成的翻译
            foreach (var language in generatedTranslationLanguages)
            {
                var importName = pathConfig.NormalizeClassName("Translation_" + language);
                var importFile = pathConfig.NormalizeFilename(language);
                includeBuilder.AppendLine(
                    $"import {{ {importName} }} from './{importFile}';");
                importedLanguages.Add(importName);
            }
            // 定义翻译列表
            classBuilder.AppendLine("export class TranslationIndex {");
            classBuilder.AppendLine("    public static translationModules = [");
            foreach (var name in importedLanguages)
            {
                classBuilder.Append($"        {name}");
                if (name != importedLanguages.Last())
                {
                    classBuilder.Append(",");
                }
                classBuilder.AppendLine();
            }
            classBuilder.AppendLine("    ];");
            classBuilder.AppendLine("}");
            includeBuilder.AppendLine();
            return includeBuilder.ToString() + classBuilder.ToString();
        }
    }
}
