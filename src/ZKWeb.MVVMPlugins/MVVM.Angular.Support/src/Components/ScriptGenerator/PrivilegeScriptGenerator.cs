using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeTranslators.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Components.ScriptGenerator {
	/// <summary>
	/// 权限的脚本生成器
	/// </summary>
	[ExportMany]
	public class PrivilegeScriptGenerator {
		/// <summary>
		/// 生成用户类型列表的脚本
		/// </summary>
		/// <returns></returns>
		public virtual string GenerateUserTypesScript() {
			// 获取系统中注册的所有用户类型和实现的接口类型
			var userTypes = ZKWeb.Application.Ioc.ResolveMany<IUserType>();
			var allTypes = new HashSet<Type>();
			foreach (var userType in userTypes) {
				var userTypeType = userType.GetType();
				foreach (var interfaceType in userTypeType.FastGetInterfaces()) {
					allTypes.Add(interfaceType);
				}
				while (userTypeType != null && userTypeType != typeof(object)) {
					allTypes.Add(userTypeType);
					userTypeType = userTypeType.GetTypeInfo().BaseType;
				}
			}
			// 生成脚本代码
			var classBuilder = new StringBuilder();
			classBuilder.AppendLine($"export class UserTypes {{");
			foreach (var type in allTypes) {
				var typeName = type.Name;
				classBuilder.AppendLine($"	/** {new T(typeName)} */");
				classBuilder.AppendLine($"	public static {typeName} = {JsonConvert.SerializeObject(typeName)};");
			}
			classBuilder.AppendLine("}");
			return classBuilder.ToString();
		}

		/// <summary>
		/// 生成权限列表的脚本
		/// </summary>
		/// <returns></returns>
		public virtual string GeneratePrivilegesScript() {
			// 获取系统中注册的所有权限
			var pathConfig = ZKWeb.Application.Ioc.Resolve<ScriptPathConfig>();
			var privilegeProviders = ZKWeb.Application.Ioc.ResolveMany<IPrivilegesProvider>();
			var privilegeTranslator = ZKWeb.Application.Ioc.Resolve<IPrivilegeTranslator>();
			var allPrivileges = privilegeProviders.SelectMany(p => p.GetPrivileges()).Distinct();
			// 生成脚本代码
			var classBuilder = new StringBuilder();
			classBuilder.AppendLine($"export class Privileges {{");
			foreach (var privilege in allPrivileges) {
				var name = privilegeTranslator.Translate(privilege);
				var variableName = pathConfig.NormalizeVariableName(privilege);
				classBuilder.AppendLine($"	/** {name} */");
				classBuilder.AppendLine($"	public static {variableName} = {JsonConvert.SerializeObject(privilege)};");
			}
			classBuilder.AppendLine("}");
			return classBuilder.ToString();
		}
	}
}
