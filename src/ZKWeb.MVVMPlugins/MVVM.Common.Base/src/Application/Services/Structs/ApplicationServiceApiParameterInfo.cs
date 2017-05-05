using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Structs
{
    /// <summary>
    /// 应用服务中的Api参数信息
    /// </summary>
    public class ApplicationServiceApiParameterInfo
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性列表
        /// </summary>
        public IEnumerable<Attribute> Attributes { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public ApplicationServiceApiParameterInfo(
            Type type, string name, IEnumerable<Attribute> attributes)
        {
            Type = type;
            Name = name;
            Attributes = attributes;
        }
    }
}
