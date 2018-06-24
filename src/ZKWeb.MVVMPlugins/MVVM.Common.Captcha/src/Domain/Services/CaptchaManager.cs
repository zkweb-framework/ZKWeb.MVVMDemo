using System;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using System.Drawing;
using System.Drawing.Drawing2D;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Captcha.src.Managers
{
    /// <summary>
    /// 验证码管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class CaptchaManager : DomainServiceBase
    {
        /// <summary>
        /// 默认的验证码位数
        /// </summary>
        public const int DefaultDigits = 4;
        /// <summary>
        /// 每个字符的宽度
        /// </summary>
        public const int ImageWidthPerChar = 20;
        /// <summary>
        /// 验证码图片的高度
        /// </summary>
        public const int ImageHeight = 32;
        /// <summary>
        /// 图片边距
        /// </summary>
        public const int ImagePadding = 5;
        /// <summary>
        /// 干扰线数量
        /// </summary>
        public const int InterferenceLines = 3;
        /// <summary>
        /// 字符图表的最大拉伸量
        /// </summary>
        public const int CharGraphicMaxPadding = 5;
        /// <summary>
        /// 字符图表的最大旋转量
        /// </summary>
        public const int CharGraphicMaxRotate = 35;
        /// <summary>
        /// 生成验证码后最小保留会话的时间
        /// </summary>
        public const int MakeSessionAliveAtLeast = 30;
        /// <summary>
        /// 保存到会话时使用的键名前缀
        /// </summary>
        public const string SessionItemKeyPrefix = "ZKWeb.Captcha.";

        /// <summary>
        /// 获取保存在会话数据中的键
        /// </summary>
        /// <param name="key">原始键</param>
        /// <returns></returns>
        protected virtual string GetSessionItemKey(string key)
        {
            // MongoDB不支持名称中有点
            return (SessionItemKeyPrefix + key).Replace('.', '_');
        }

        /// <summary>
        /// 生成验证码并储存验证码到会话中
        /// </summary>
        /// <param name="key">使用的键名</param>
        /// <param name="digits">验证码位数</param>
        /// <param name="chars">使用的字符列表</param>
        /// <returns></returns>
        public virtual Image Generate(string key, int digits = DefaultDigits, string chars = null)
        {
            // 生成验证码
            var captchaCode = RandomUtils.RandomString(
                digits, chars ?? "23456789ABCDEFGHJKLMNPQRSTUWXYZ");
            var image = new Bitmap(ImageWidthPerChar * digits + ImagePadding * 2, ImageHeight);
            var font = new Font("Arial", ImageWidthPerChar, FontStyle.Bold);
            var rand = RandomUtils.Generator;
            using (var graphic = Graphics.FromImage(image))
            {
                // 描画背景
                var backgroundBrush = new SolidBrush(Color.White);
                graphic.FillRectangle(backgroundBrush, new RectangleF(0, 0, image.Width, image.Height));
                // 添加干扰线
                var pen = new Pen(Color.Black);
                for (int x = 0; x < InterferenceLines; ++x)
                {
                    var pointStart = new Point(rand.Next(image.Width), rand.Next(image.Height));
                    var pointFinish = new Point(rand.Next(image.Width), rand.Next(image.Height));
                    graphic.DrawLine(pen, pointStart, pointFinish);
                }
                // 逐个字符描画，并进行不规则拉伸
                var stringFormat = StringFormat.GenericDefault;
                var randomPadding = new Func<int>(() => rand.Next(CharGraphicMaxPadding));
                var textBrush = new SolidBrush(Color.Black);
                for (int x = 0; x < captchaCode.Length; ++x)
                {
                    var path = new GraphicsPath();
                    var rect = new RectangleF(
                        ImageWidthPerChar * x + ImagePadding, ImagePadding,
                        ImageWidthPerChar, image.Height - ImagePadding);
                    var str = captchaCode[x].ToString();
                    path.AddString(str, font.FontFamily, (int)font.Style, font.Size, rect, stringFormat);
                    // 拉伸
                    var points = new PointF[] {
                        new PointF(rect.X + randomPadding(), randomPadding()),
                        new PointF(rect.X + rect.Width - randomPadding(), randomPadding()),
                        new PointF(rect.X + randomPadding(), image.Height - randomPadding()),
                        new PointF(rect.X + rect.Width - randomPadding(), image.Height - randomPadding()),
                    };
                    path.Warp(points, rect);
                    // 旋转
                    var matrix = new Matrix();
                    matrix.RotateAt(rand.Next(-CharGraphicMaxRotate, CharGraphicMaxRotate), rect.Location);
                    graphic.Transform = matrix;
                    // 描画到图层
                    graphic.FillPath(textBrush, path);
                }
            }
            // 储存到会话，会话的过期时间最少是30分钟
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            session[GetSessionItemKey(key)] = captchaCode;
            session.SetExpiresAtLeast(TimeSpan.FromMinutes(MakeSessionAliveAtLeast));
            sessionManager.SaveSession();
            // 返回图片对象
            return image;
        }

        /// <summary>
        /// 获取当前验证码，但不删除
        /// 这个函数一般只用于单元测试或语音提示，检查请使用Check函数
        /// </summary>
        /// <param name="key">使用的键名</param>
        /// <returns></returns>
        public virtual string GetWithoutRemove(string key)
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            return session[GetSessionItemKey(key)].ConvertOrDefault<string>();
        }

        /// <summary>
        /// 检查验证码是否正确，检查后删除原验证码
        /// </summary>
        /// <param name="key">使用的键名</param>
        /// <param name="actualCode">收到的验证码</param>
        /// <returns></returns>
        public virtual bool Check(string key, string actualCode)
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            var session = sessionManager.GetSession();
            var itemKey = GetSessionItemKey(key);
            var exceptedCode = session[itemKey].ConvertOrDefault<string>();
            session[itemKey] = null;
            sessionManager.SaveSession();
            return !string.IsNullOrEmpty(exceptedCode) &&
                !string.IsNullOrEmpty(actualCode) &&
                actualCode.ToLower() == exceptedCode.ToLower();
        }
    }
}
