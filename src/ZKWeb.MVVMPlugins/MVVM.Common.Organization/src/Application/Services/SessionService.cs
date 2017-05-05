using AutoMapper;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Services
{
    /// <summary>
    /// 会话服务
    /// 用于获取当前登录的用户信息
    /// </summary>
    [ExportMany, SingletonReuse, Description("会话服务")]
    public class SessionService : ApplicationServiceBase
    {
        private SessionManager _sessionManager;

        public SessionService(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// 获取当前的会话信息
        /// </summary>
        /// <returns></returns>
        [Description("获取当前的会话信息")]
        public virtual SessionInfoDto GetSessionInfo()
        {
            var session = _sessionManager.GetSession();
            var user = session.GetUser();
            var result = new SessionInfoDto();
            if (user != null)
            {
                result.User = Mapper.Map<UserOutputDto>(user);
            }
            return result;
        }
    }
}
