using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes {
	/// <summary>
	/// 检查文件的验证属性
	/// </summary>
	public class CheckFileAttribute : ValidationAttribute {
		/// <summary>
		/// 允许的扩展名
		/// 默认允许图片
		/// </summary>
		public string AllowedExtensions { get; set; }
		/// <summary>
		/// 最大的文件大小
		/// 默认是1MB
		/// </summary>
		public int MaximumSize { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CheckFileAttribute() : this(".jpg,.bmp,.gif,.png", 1024 * 1024) {

		}

		/// <summary>
		/// 初始化
		/// </summary>
		public CheckFileAttribute(string allowedExtensions, int maximumSize) {
			AllowedExtensions = allowedExtensions.ToLower();
			MaximumSize = maximumSize;
		}

		/// <summary>
		/// 检查是否合法
		/// 等于null时会通过
		/// 如果文件必填请使用Required属性
		/// </summary>
		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			}
			var file = (IHttpPostedFile)value;
			var extension = Path.GetExtension(file.FileName).ToLower();
			var allowedExtensions = AllowedExtensions.Split(',');
			if (!allowedExtensions.Contains(extension)) {
				throw new BadRequestException("File extension is not allowed");
			}
			if (file.Length > MaximumSize) {
				var msg = new T(
					"File is too big, the maximum size allowed is {0}",
					FileUtils.GetSizeDisplayName(MaximumSize));
				throw new BadRequestException(msg);
			}
			return true;
		}
	}
}
