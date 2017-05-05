using System;
using System.Collections;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWebStandard.Utils;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions
{
    /// <summary>
    /// 用户的扩展函数
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// 设置密码
        /// </summary>
        public static void SetPassword(this User user, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }
            user.SetPasswordInfo(PasswordInfo.FromPassword(password));
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        public static bool CheckPassword(this User user, string password)
        {
            var passwordInfo = user.GetPasswordInfo();
            if (string.IsNullOrEmpty(passwordInfo.Hash))
            {
                return false;
            }
            return passwordInfo.Check(password);
        }

        /// <summary>
        /// 获取用户类型对象
        /// </summary>
        public static IUserType GetUserType(this User user)
        {
            var type = ZKWeb.Application.Ioc.ResolveMany<IUserType>()
                .FirstOrDefault(t => t.Type == user?.Type);
            if (type == null)
            {
                throw new NotSupportedException(string.Format(
                    "Unsupported user type: {0}", user.Type));
            }
            return type;
        }

        /// <summary>
        /// 获取已实现的用户类型列表
        /// </summary>
        public static IEnumerable<Type> GetImplementedUserTypes(this User user)
        {
            var result = new HashSet<Type>();
            var type = user.GetUserType().GetType();
            foreach (var interfaceType in type.FastGetInterfaces())
            {
                result.Add(interfaceType);
            }
            while (type != null && type != typeof(object))
            {
                result.Add(type);
                type = type.GetTypeInfo().BaseType;
            }
            return result;
        }

        /// <summary>
        /// 获取用户拥有的权限列表
        /// </summary>
        public static IEnumerable<string> GetPrivileges(this User user)
        {
            return user.Roles.SelectMany(r => r.To.GetPrivileges()).Distinct();
        }
    }
}
