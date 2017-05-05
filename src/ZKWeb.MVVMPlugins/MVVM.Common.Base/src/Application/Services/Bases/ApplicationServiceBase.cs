using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Structs;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases
{
    /// <summary>
    /// 应用服务的基础类
    /// 继承这个类以后，所有公有的函数都会运行在工作单元中
    /// 并且公有函数会对应一个Api地址，格式是"/api/类名/函数名"
    /// </summary>
    public class ApplicationServiceBase :
        IController,
        IApplicationService,
        IWebsiteStartHandler
    {
        /// <summary>
        /// 当前的Http上下文
        /// </summary>
        protected virtual IHttpContext Context => HttpManager.CurrentContext;
        /// <summary>
        /// 当前的Http请求
        /// </summary>
        protected virtual IHttpRequest Request => Context.Request;
        /// <summary>
        /// 当前的Http回应
        /// </summary>
        protected virtual IHttpResponse Response => Context.Response;
        /// <summary>
        /// 当前使用的工作单元
        /// </summary>
        protected virtual IUnitOfWork UnitOfWork => ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
        /// <summary>
        /// 基础地址
        /// </summary>
        protected virtual string UrlBase => $"/api/{GetType().Name}";

        /// <summary>
        /// 获取Api函数列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ApplicationServiceApiMethodInfo> GetApiMethods()
        {
            var typeInfo = GetType().GetTypeInfo();
            var methods = typeInfo.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.IsSpecialName)
                    continue;
                if (method.ReturnType == typeof(void))
                    continue;
                // 创建函数委托
                // 如果函数未标记[UnitOfWork]则手动包装该函数
                var action = this.BuildActionDelegate(method);
                if (method.GetCustomAttribute<UnitOfWorkAttribute>() == null)
                {
                    action = new UnitOfWorkAttribute().Filter(action);
                }
                // 包装过滤器
                var filterAttributes = method.GetCustomAttributes<ActionFilterAttribute>();
                foreach (var filterAttribute in filterAttributes)
                {
                    action = filterAttribute.Filter(action);
                }
                // 返回函数信息
                var info = new ApplicationServiceApiMethodInfo(
                    method.ReturnType,
                    method.Name,
                    $"{UrlBase}/{method.Name}",
                    method.FastGetCustomAttributes(typeof(Attribute), true).OfType<Attribute>(),
                    method.GetParameters().Select(p => new ApplicationServiceApiParameterInfo(
                        p.ParameterType,
                        p.Name,
                        p.GetCustomAttributes(typeof(Attribute), true).OfType<Attribute>())),
                    action);
                yield return info;
            }
        }

        /// <summary>
        /// 网站启动时注册所有Api函数
        /// </summary>
        public void OnWebsiteStart()
        {
            var typeInfo = GetType().GetTypeInfo();
            var methods = typeInfo.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            var controllerManager = ZKWeb.Application.Ioc.Resolve<ControllerManager>();
            foreach (var methodInfo in GetApiMethods())
            {
                var url = methodInfo.Url;
                var action = methodInfo.Action;
                controllerManager.RegisterAction(url, HttpMethods.POST, action);
            }
        }
    }
}
