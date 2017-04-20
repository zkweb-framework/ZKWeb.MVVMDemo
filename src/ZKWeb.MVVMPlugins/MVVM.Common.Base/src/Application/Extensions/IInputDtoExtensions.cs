using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.FastReflection;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Extensions {
	/// <summary>
	/// 输入传输对象的扩展函数
	/// </summary>
	public static class IInputDtoExtensions {
		/// <summary>
		/// 检查数据传输对象是否合法，不合法时抛出例外
		/// </summary>
		public static void Validate(this IInputDto inputDto) {
			string message;
			if (!inputDto.IsValid(out message)) {
				throw new FormatException(message);
			}
		}

		/// <summary>
		/// 检查数据传输对象是否合法
		/// </summary>
		public static bool IsValid(this IInputDto inputDto) {
			string message;
			return IsValid(inputDto, out message);
		}

		/// <summary>
		/// 检查数据传输对象是否合法，不合法时返回错误消息
		/// </summary>
		public static bool IsValid(this IInputDto inputDto, out string message) {
			message = "";
			var properties = inputDto.GetType().FastGetProperties();
			foreach (var property in properties) {
				if (!property.CanRead) {
					continue;
				}
				var attributes = property.FastGetCustomAttributes(typeof(ValidationAttribute));
				if (attributes.Length == 0) {
					continue;
				}
				var value = property.FastGetValue(inputDto);
				var propertyName = new T(property
					.FastGetCustomAttributes(typeof(DescriptionAttribute))
					.OfType<DescriptionAttribute>()
					.FirstOrDefault()?.Description ?? property.Name);
				foreach (var attribute in attributes.OfType<ValidationAttribute>()) {
					if (!attribute.IsValid(value)) {
						message += $"{attribute.FormatErrorMessage(propertyName)}\r\n";
					}
				}
			}
			return string.IsNullOrEmpty(message);
		}
	}
}
