using System.IO;
using ZKWeb.Storage;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Angular.Support.src.Application
{
    /// <summary>
    /// 负责处理AngularJS相关的请求
    /// </summary>
    [ExportMany, SingletonReuse]
    public class AngularRouter : IHttpRequestHandler
    {
        protected string _apiPrefix;
        protected IFileStorage _fileStorage;

        /// <summary>
        /// 初始化
        /// </summary>
        public AngularRouter(IFileStorage fileStorage)
        {
            _apiPrefix = "/api/";
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        public void OnRequest()
        {
            var context = HttpManager.CurrentContext;
            // 不处理api请求
            if (context.Request.Path.StartsWith(_apiPrefix))
            {
                return;
            }
            // 查找对应的文件
            var path = context.Request.Path.Substring(1);
            var contentType = "";
            IFileEntry fileEntry = null;
            if (string.IsNullOrEmpty(path))
            {
                // 请求的是首页
                fileEntry = _fileStorage.GetResourceFile("static", "dist", "index.html");
            }
            else
            {
                // 请求的是子页或资源文件
                var extension = Path.GetExtension(path);
                if (string.IsNullOrEmpty(extension))
                {
                    // 请求的是子页
                    fileEntry = _fileStorage.GetResourceFile("static", "dist", "index.html");
                }
                else
                {
                    // 请求的是资源文件，首先判断是否有对应的预压缩文件
                    fileEntry = _fileStorage.GetResourceFile("static", "dist", path + ".gz");
                    if (fileEntry.Exists && context.Request.GetHeader("Accept-Encoding").Contains("gzip"))
                    {
                        // 返回预压缩文件，需要使用原来的Content-Type
                        context.Response.AddHeader("Content-Encoding", "gzip");
                        contentType = MimeUtils.GetMimeType(path);
                    }
                    else
                    {
                        // 原路径存在则使用原路径内容，否则不处理
                        fileEntry = _fileStorage.GetResourceFile("static", "dist", path);
                    }
                }
            }
            // 返回文件内容
            if (fileEntry.Exists)
            {
                var result = new FileEntryResult(fileEntry, context.Request.GetIfModifiedSince());
                if (!string.IsNullOrEmpty(contentType))
                {
                    result.ContentType = contentType;
                }
                result.WriteResponse(context.Response);
                context.Response.End();
            }
        }
    }
}
