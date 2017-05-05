using System;
using System.ComponentModel;
using System.DrawingCore.Imaging;
using System.IO;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Managers;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Application.Services
{
    /// <summary>
    /// 验证码服务
    /// </summary>
    [ExportMany, SingletonReuse, Description("验证码服务")]
    public class CaptchaService : ApplicationServiceBase
    {
        private CaptchaManager _captchaManager;

        public CaptchaService(CaptchaManager captchaManager)
        {
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 获取验证码图片的Base64
        /// </summary>
        /// <param name="key">使用的键名</param>
        /// <returns></returns>
        [Description("获取验证码图片的Base64")]
        public string GetCaptchaImageBase64(string key)
        {
            using (var stream = new MemoryStream())
            using (var image = _captchaManager.Generate(key))
            {
                image.Save(stream, ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
