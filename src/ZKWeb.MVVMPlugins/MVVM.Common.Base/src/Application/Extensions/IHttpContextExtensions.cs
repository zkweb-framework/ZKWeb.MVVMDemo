using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Structs;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Extensions
{
    /// <summary>
    /// Http上下文的扩展函数
    /// </summary>
    public static class IHttpContextExtensions
    {
        /// <summary>
        /// 设置当前的Api函数信息
        /// </summary>
        public static void SetApiMethodInfo(this IHttpContext context, ApplicationServiceApiMethodInfo info)
        {
            context.Items[typeof(ApplicationServiceApiMethodInfo)] = info;
        }

        /// <summary>
        /// 获取当前的Api函数信息
        /// </summary>
        public static ApplicationServiceApiMethodInfo GetApiMethodInfo(this IHttpContext context)
        {
            var info = context.Items.GetOrDefault(typeof(ApplicationServiceApiMethodInfo));
            return (ApplicationServiceApiMethodInfo)info;
        }
    }
}
