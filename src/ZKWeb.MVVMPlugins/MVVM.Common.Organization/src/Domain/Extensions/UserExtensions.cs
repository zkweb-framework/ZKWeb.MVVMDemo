using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions {
	/// <summary>
	/// 用户的扩展函数
	/// </summary>
	public static class UserExtensions {
		/// <summary>
		/// 设置密码
		/// </summary>
		public static void SetPassword(this User user, string password) {
			if (string.IsNullOrEmpty(password)) {
				throw new ArgumentNullException("password");
			}
			user.Password = PasswordInfo.FromPassword(password);
		}

		/// <summary>
		/// 检查密码
		/// </summary>
		public static bool CheckPassword(this User user, string password) {
			if (user.Password == null || string.IsNullOrEmpty(password)) {
				return false;
			}
			return user.Password.Check(password);
		}

		/// <summary>
		/// 获取用户类型对象
		/// </summary>
		public static IUserType GetUserType(this User user) {
			var type = Application.Ioc.ResolveMany<IUserType>()
				.FirstOrDefault(t => t.Type == user?.Type);
			if (type == null) {
				throw new NotSupportedException(string.Format(
					"Unsupported user type: {0}", user.Type));
			}
			return type;
		}
	}
}
