using System;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Entities;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Extensions
{
    /// <summary>
    /// 会话的扩展函数
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// 用于在当前Http请求当前会话对应的租户
        /// </summary>
        public const string SessionTenantContextKey = "ZKWeb.SessionTenant";

        /// <summary>
        /// 获取会话对应的租户
        /// </summary>
        public static Tenant GetTenant(this Session session)
        {
            // 会话没有对应租户
            if (session.TenantId == null)
            {
                return null;
            }
            // 从Http上下文中获取，确保保存时的会话和获取时的会话是同一个
            if (HttpManager.CurrentContextExists)
            {
                var context = HttpManager.CurrentContext;
                var pair = context.GetData<Tuple<Session, Tenant>>(SessionTenantContextKey);
                if (pair != null && pair.Item1 == session)
                {
                    return pair.Item2;
                }
            }
            // 从服务获取
            var service = Application.Ioc.Resolve<IDomainService<Tenant, Guid>>();
            var user = service.Get(session.TenantId.Value);
            if (HttpManager.CurrentContextExists)
            {
                var context = HttpManager.CurrentContext;
                context.PutData(SessionTenantContextKey, Tuple.Create(session, user));
            }
            return user;
        }

        /// <summary>
        /// 重新生成Id
        /// </summary>
        public static void ReGenerateId(this Session session)
        {
            session.Id = GuidUtils.SecureSequentialGuid(DateTime.UtcNow);
        }

        /// <summary>
        /// 设置会话最少在指定的时间后过期
        /// 当前会话的过期时间比指定的时间要晚时不更新当前的过期时间
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="span">最少在这个时间后过期</param>
        public static void SetExpiresAtLeast(this Session session, TimeSpan span)
        {
            var expires = DateTime.UtcNow + span;
            if (session.Expires < expires)
            {
                session.Expires = expires;
            }
        }
    }
}
