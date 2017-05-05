using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ValidationMessageProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Extensions
{
    /// <summary>
    /// 输入传输对象的扩展函数
    /// </summary>
    public static class IInputDtoExtensions
    {
        /// <summary>
        /// 格式化验证错误
        /// </summary>
        /// <typeparam name="T">验证属性类型</typeparam>
        /// <param name="attribute">验证属性</param>
        /// <param name="name">成员名称</param>
        /// <returns></returns>
        private static string FormatValidationError<T>(T attribute, string name)
            where T : ValidationAttribute
        {
            var provider = ZKWeb.Application.Ioc
                .Resolve<IValidationMessageProvider<T>>(IfUnresolved.ReturnDefault);
            if (provider != null)
            {
                return provider.FormatErrorMessage(attribute, name);
            }
            return attribute.FormatErrorMessage(name);
        }

        /// <summary>
        /// FormatValidationError的MethodInfo
        /// </summary>
        private static MethodInfo FormatValidationErrorMethod = typeof(IInputDtoExtensions)
            .FastGetMethod(nameof(FormatValidationError), BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// 检查数据传输对象是否合法
        /// </summary>
        public static bool IsValid(this IInputDto inputDto)
        {
            IList<string> errors;
            return IsValid(inputDto, out errors);
        }

        /// <summary>
        /// 检查数据传输对象是否合法，不合法时返回错误消息
        /// </summary>
        public static bool IsValid(this IInputDto inputDto, out IList<string> errors)
        {
            errors = new List<string>();
            var properties = inputDto.GetType().FastGetProperties();
            foreach (var property in properties)
            {
                if (!property.CanRead)
                {
                    continue;
                }
                var attributes = property.FastGetCustomAttributes(typeof(ValidationAttribute));
                if (attributes.Length == 0)
                {
                    continue;
                }
                var value = property.FastGetValue(inputDto);
                // 获取属性名称
                var propertyName = new T(property
                    .FastGetCustomAttributes(typeof(DescriptionAttribute))
                    .OfType<DescriptionAttribute>()
                    .FirstOrDefault()?.Description ?? property.Name).ToString();
                // 检查标记在属性上的验证属性
                foreach (var attribute in attributes.OfType<ValidationAttribute>())
                {
                    if (attribute.IsValid(value))
                    {
                        continue;
                    }
                    errors.Add((string)FormatValidationErrorMethod
                        .MakeGenericMethod(attribute.GetType())
                        .FastInvoke(null, attribute, propertyName));
                }
            }
            return errors.Count == 0;
        }

        /// <summary>
        /// 检查数据传输对象是否合法，不合法时抛出例外
        /// </summary>
        public static void Validate(this IInputDto inputDto)
        {
            IList<string> errors;
            if (!inputDto.IsValid(out errors))
            {
                throw new BadRequestException(string.Join("\r\n", errors));
            }
        }
    }
}
