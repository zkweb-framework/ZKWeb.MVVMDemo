using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.CORSExposeHeaders.Interfaces;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.RequestHandlers
{
    /// <summary>
    /// 处理跨站请求的处理器
    /// </summary>
    [ExportMany]
    public class CorsRequestHandler : IHttpRequestPreHandler
    {
        public void OnRequest()
        {
            var context = HttpManager.CurrentContext;
            // 允许不使用OPTIONS直接发来的请求
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            // 指定允许客户端获取的头列表
            var exposeHeaders = ZKWeb.Application.Ioc.ResolveMany<ICORSExposeHeader>();
            context.Response.AddHeader("Access-Control-Expose-Headers",
                string.Join(",", exposeHeaders.Select(e => e.ExposeHeader.ToLower())));
            // 允许使用OPTIONS探测的请求
            if (context.Request.Method == "OPTIONS")
            {
                var requestHeaders = context.Request.GetHeader("Access-Control-Request-Headers");
                var requestMethod = context.Request.GetHeader("Access-Control-Request-Method");
                context.Response.AddHeader("Access-Control-Allow-Headers", requestHeaders);
                context.Response.AddHeader("Access-Control-Allow-Methods", requestMethod);
                context.Response.AddHeader("Access-Control-Max-Age", "1296000");
                context.Response.StatusCode = 200;
                context.Response.End();
            }
        }
    }
}
