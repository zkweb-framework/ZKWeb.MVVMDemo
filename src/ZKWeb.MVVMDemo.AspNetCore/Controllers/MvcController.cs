using Microsoft.AspNetCore.Mvc;
using System;
using ZKWeb.Storage;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMDemo.AspNetCore.Controllers
{
    /// <summary>
    /// Mvc控制器
    /// </summary>
    public class MvcController : Controller
    {
        /// <summary>
        /// 文件储存, 来源于ZKWeb的服务
        /// </summary>
        private IFileStorage _fileStorage;

        /// <summary>
        /// 初始化
        /// </summary>
        public MvcController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// 实验用的页面, 路径"/Mvc/Index"
        /// </summary>
        public IActionResult Index()
        {
            return Content("hello mvc");
        }

        /// <summary>
        /// 测试读取和写入文件, 路径"/Mvc/ReadWriteFile"
        /// </summary>
        public IActionResult ReadWriteFile()
        {
            var file = _fileStorage.GetStorageFile("mvc.txt");
            file.WriteAllText(DateTime.UtcNow.ToString());
            return Content(file.ReadAllText());
        }
    }
}
