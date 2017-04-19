using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// 数据传输对象的脚本生成器
	/// </summary>
	[ExportMany]
	public class DtoScriptGenerator {
		/// <summary>
		/// 获取脚本中的类型
		/// 如果类型需要生成脚本，添加类型到已发现的类型中
		/// </summary>
		public virtual string GetScriptType(Type type, IList<Type> discoveredTypes) {
			// 拆开Nullable
			type = Nullable.GetUnderlyingType(type) ?? type;
			// 判断是否布尔
			if (type == typeof(bool)) {
				return "boolean";
			}
			// 判断是否数值
			if (type == typeof(sbyte) || type == typeof(byte) ||
				type == typeof(short) || type == typeof(ushort) ||
				type == typeof(int) || type == typeof(uint) ||
				type == typeof(long) || type == typeof(ulong) ||
				type == typeof(float) || type == typeof(double) ||
				type == typeof(decimal)) {
				return "number";
			}
			// 判断是否字符串
			if (type == typeof(char) ||
				type == typeof(string) ||
				type == typeof(Guid) ||
				type == typeof(DateTime)) {
				return "string";
			}
			// 判断是否数组
			// 支持数组嵌套数组，但不支持多维数组
			var typeInfo = type.GetTypeInfo();
			if (typeInfo.IsArray) {
				var elementType = GetScriptType(
					typeInfo.GetElementType(), discoveredTypes);
				return $"{elementType}[]";
			}
			// 判断是否字典
			var dictionaryInterface = typeInfo.GetInterfaces().FirstOrDefault(x =>
				x.GetTypeInfo().IsGenericType &&
				x.GetTypeInfo().GetGenericTypeDefinition() == typeof(IDictionary<,>));
			if (dictionaryInterface != null) {
				var keyType = GetScriptType(typeInfo.GetGenericArguments()[0], discoveredTypes);
				var valueType = GetScriptType(typeInfo.GetGenericArguments()[1], discoveredTypes);
				return $"{{ [key: {keyType}]: {valueType} }}";
			}
			// 判断是否集合
			var collectionInterface = typeInfo.GetInterfaces().FirstOrDefault(x =>
				x.GetTypeInfo().IsGenericType &&
				x.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICollection<>));
			if (collectionInterface != null) {
				var elementType = GetScriptType(
					collectionInterface.GetGenericArguments()[0], discoveredTypes);
				return $"{elementType}[]";
			}
			// 判断是否其他数据传输对象
			if (typeInfo.IsEnum ||
				(typeInfo.IsClass && typeInfo.Assembly != typeof(string).GetTypeInfo().Assembly)) {
				discoveredTypes.Add(type);
				var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
				return pathConfig.NormalizeClassName(type);
			}
			// 其他类型都归到any
			return "any";
		}

		/// <summary>
		/// 判断是否应该生成此类型的脚本
		/// </summary>
		/// <returns></returns>
		public virtual bool ShouldGenerate(Type type) {
			var discoveredTypes = new List<Type>();
			GetScriptType(type, discoveredTypes);
			return discoveredTypes.Count > 0;
		}

		/// <summary>
		/// 生成类型是class的脚本
		/// </summary>
		protected virtual string GenerateClassScript(Type type, IList<Type> discoveredTypes) {
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var includeBuilder = new StringBuilder();
			var classBuilder = new StringBuilder();
			var includedTypes = new HashSet<Type>() { type };
			var className = pathConfig.NormalizeClassName(type);
			var classDescription = type.GetTypeInfo()
				.GetCustomAttribute<DescriptionAttribute>()?.Description ?? className;
			classBuilder.AppendLine($"// {classDescription}");
			classBuilder.AppendLine($"export class {className} {{");
			foreach (var property in type.GetProperties()) {
				// 成员带有[JsonIgnore]时跳过生成
				if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null) {
					continue;
				}
				// 获取成员信息
				var newDiscoveredTypes = new List<Type>();
				var propertyName = property.Name;
				var propertyType = GetScriptType(property.PropertyType, newDiscoveredTypes);
				var propertyDescription = property
					.GetCustomAttribute<DescriptionAttribute>()?.Description ?? propertyName;
				// 必要时引用其他类型的脚本
				foreach (var newDiscoveredType in newDiscoveredTypes) {
					if (!includedTypes.Contains(newDiscoveredType)) {
						var importName = pathConfig.NormalizeClassName(newDiscoveredType);
						var importFile = pathConfig.NormalizeFilename(importName);
						includeBuilder.AppendLine($"import {{ {importName} }} from './{importFile}';");
						includedTypes.Add(newDiscoveredType);
						discoveredTypes.Add(newDiscoveredType);
					}
				}
				// 添加成员
				classBuilder.AppendLine($"	// {propertyDescription}");
				classBuilder.AppendLine($"	public {propertyName}: {propertyType};");
			}
			classBuilder.AppendLine("}");
			if (includeBuilder.Length > 0) {
				includeBuilder.AppendLine();
			}
			return includeBuilder.ToString() + classBuilder.ToString();
		}

		/// <summary>
		/// 生成类型是enum的脚本
		/// </summary>
		protected virtual string GenerateEnumScript(Type type) {
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var enumBuilder = new StringBuilder();
			var enumName = pathConfig.NormalizeClassName(type);
			var enumDescription = type.GetTypeInfo()
				.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumName;
			enumBuilder.AppendLine($"// {enumDescription}");
			enumBuilder.AppendLine($"export enum {enumName} {{");
			var enumValues = Enum.GetValues(type).OfType<Enum>().ToList();
			foreach (var value in enumValues) {
				var valueName = value.ToString();
				var valueInt = value.ConvertOrDefault<int>();
				var valueDescription = value.GetDescription();
				enumBuilder.AppendLine($"	// {valueDescription}");
				enumBuilder.Append($"	{valueName} = {valueInt}");
				if (value != enumValues.Last()) {
					enumBuilder.Append(",");
				}
				enumBuilder.AppendLine();
			}
			enumBuilder.AppendLine("}");
			return enumBuilder.ToString();
		}

		/// <summary>
		/// 根据对象类型生成脚本
		/// </summary>
		public virtual string GenerateScript(Type type, IList<Type> discoveredTypes) {
			if (type.GetTypeInfo().IsEnum) {
				return GenerateEnumScript(type);
			} else {
				return GenerateClassScript(type, discoveredTypes);
			}
		}
	}
}
