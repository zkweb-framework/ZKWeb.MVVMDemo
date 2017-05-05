using Microsoft.AspNetCore.Mvc;

namespace ZKWeb.MVVMDemo.AspNetCore.Controllers
{
    /// <summary>
    /// Mvc控制器
    /// </summary>
    public class MvcController : Controller
    {
        /// <summary>
        /// 实验用的页面
        /// 访问/mvc/index即可看到
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Content("hello mvc");
        }
    }
}
