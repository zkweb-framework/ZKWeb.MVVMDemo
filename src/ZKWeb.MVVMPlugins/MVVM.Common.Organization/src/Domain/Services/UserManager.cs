using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Exceptions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Filters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ExtraConfigKeys;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.UserLoginHandlers.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.SessionState.src.Domain.Services;
using ZKWeb.Server;
using ZKWeb.Storage;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Services
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    [ExportMany, SingletonReuse]
    public class UserManager : DomainServiceBase<User, Guid>
    {
        /// <summary>
        /// 记住登陆时，保留会话的天数
        /// 默认30天，可通过网站配置指定
        /// </summary>
        public TimeSpan SessionExpireDaysWithRememebrLogin { get; set; }
        /// <summary>
        /// 不记住登陆时，保留会话的天数
        /// 默认1天，可通过网站配置指定
        /// </summary>
        public TimeSpan SessionExpireDaysWithoutRememberLogin { get; set; }
        /// <summary>
        /// 头像宽度
        /// 默认150，可通过网站配置指定
        /// </summary>
        public int AvatarWidth { get; set; }
        /// <summary>
        /// 头像高度
        /// 默认150，可通过网站配置指定
        /// </summary>
        public int AvatarHeight { get; set; }
        /// <summary>
        /// 头像图片质量，默认90
        /// </summary>
        public int AvatarImageQuality { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public UserManager()
        {
            var configManager = ZKWeb.Application.Ioc.Resolve<WebsiteConfigManager>();
            var extra = configManager.WebsiteConfig.Extra;
            SessionExpireDaysWithRememebrLogin = TimeSpan.FromDays(
                extra.GetOrDefault(OrganizationExtraConfigKeys.SessionExpireDaysWithRememebrLogin, 30));
            SessionExpireDaysWithoutRememberLogin = TimeSpan.FromDays(
                extra.GetOrDefault(OrganizationExtraConfigKeys.SessionExpireDaysWithoutRememberLogin, 1));
            AvatarWidth = extra.GetOrDefault(OrganizationExtraConfigKeys.AvatarWidth, 150);
            AvatarHeight = extra.GetOrDefault(OrganizationExtraConfigKeys.AvatarHeight, 150);
            AvatarImageQuality = 90;
        }

        /// <summary>
        /// 保存用户时检查用户名是否重复
        /// </summary>
        public override void Save(ref User entity, Action<User> update = null)
        {
            using (UnitOfWork.Scope())
            using (UnitOfWork.DisableFilter(typeof(DeletedFilter)))
            {
                UnitOfWork.Context.BeginTransaction();
                var e = entity;
                if (Repository.Count(u =>
                    u.Username == e.Username && u.OwnerTenantId == e.OwnerTenantId && u.Id != e.Id) > 0)
                {
                    throw new BadRequestException("Username has been taken");
                }
                base.Save(ref entity, update);
                UnitOfWork.Context.FinishTransaction();
            }
        }

        /// <summary>
        /// 根据用户名查找用户
        /// 找不到时返回null
        /// </summary>
        public virtual User FindUser(
            string tenant,
            string username)
        {
            var uow = UnitOfWork;
            var handlers = ZKWeb.Application.Ioc.ResolveMany<IUserLoginHandler>();
            User user = null;
            using (uow.Scope())
            using (uow.DisableFilter(typeof(OwnerTenantFilter)))
            {
                // 通过处理器查找用户
                foreach (var handler in handlers)
                {
                    user = handler.FindUser(tenant, username);
                    if (user != null)
                    {
                        return user;
                    }
                }
                // 通过用户名查找用户
                // 默认过滤器会过滤已删除的用户
                user = Get(u => u.Username == username && u.OwnerTenant.Name == tenant);
            }
            return user;
        }

        /// <summary>
        /// 登陆用户
        /// 登陆失败时会抛出例外
        /// </summary>
        public virtual void Login(
            string tenant,
            string username,
            string password,
            bool rememberLogin)
        {
            // 用户不存在或密码错误时抛出例外
            var user = FindUser(tenant, username);
            if (user == null || !user.CheckPassword(password))
            {
                throw new ForbiddenException("Incorrect username or password");
            }
            // 以指定用户登录
            LoginWithUser(user, rememberLogin);
        }

        /// <summary>
        /// 以指定用户登录
        /// 跳过密码等检查
        /// </summary>
        public virtual void LoginWithUser(User user, bool rememberLogin)
        {
            // 获取回调
            var handlers = ZKWeb.Application.Ioc.ResolveMany<IUserLoginHandler>().ToList();
            // 登陆前的处理
            handlers.ForEach(c => c.BeforeLogin(user));
            // 设置会话
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            sessionManager.RemoveSession(false);
            var session = sessionManager.GetSession();
            session.ReGenerateId();
            session.UserId = user.Id;
            session.TenantId = user.OwnerTenantId;
            session.RememberLogin = rememberLogin;
            session.SetExpiresAtLeast(
                session.RememberLogin ?
                SessionExpireDaysWithRememebrLogin :
                SessionExpireDaysWithoutRememberLogin);
            sessionManager.SaveSession();
            // 登陆后的处理
            handlers.ForEach(c => c.AfterLogin(user));
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public virtual void Logout()
        {
            var sessionManager = ZKWeb.Application.Ioc.Resolve<SessionManager>();
            sessionManager.RemoveSession(true);
        }

        /// <summary>
        /// 获取用户头像的储存路径，文件不一定存在
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public virtual IFileEntry GetAvatarStorageFile(Guid userId)
        {
            var fileStorage = ZKWeb.Application.Ioc.Resolve<IFileStorage>();
            return fileStorage.GetStorageFile(
                "static", "mvvm.common.organization.images", string.Format("avatar_{0}.jpg", userId));
        }

        /// <summary>
        /// 保存头像，返回是否成功和错误信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="imageStream">图片数据流</param>
        public virtual void SaveAvatar(Guid userId, Stream imageStream)
        {
            if (imageStream == null)
            {
                throw new BadRequestException("Please select avatar file");
            }
            Image image;
            try
            {
                image = Image.FromStream(imageStream);
            }
            catch
            {
                throw new BadRequestException("Parse uploaded image failed");
            }
            using (image)
            {
                var fileEntry = GetAvatarStorageFile(userId);
                using (var newImage = image.Resize(
                    AvatarWidth, AvatarHeight, ImageResizeMode.Padding, Color.White))
                {
                    using (var stream = fileEntry.OpenWrite())
                    {
                        newImage.SaveAuto(stream, ".jpg", AvatarImageQuality);
                    }
                }
            }
        }

        /// <summary>
        /// 删除头像
        /// </summary>
        /// <param name="userId">用户Id</param>
        public virtual void DeleteAvatar(Guid userId)
        {
            GetAvatarStorageFile(userId).Delete();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        public virtual void ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            using (UnitOfWork.Scope())
            {
                var user = Get(userId);
                if (user == null)
                {
                    throw new ForbiddenException("User not found");
                }
                else if (!user.CheckPassword(oldPassword))
                {
                    throw new ForbiddenException("Incorrect old password");
                }
                Save(ref user, u => u.SetPassword(newPassword));
            }
        }

        /// <summary>
        /// 给用户分配角色列表
        /// </summary>
        public virtual void AssignRoles(User user, IList<Guid> roleIds)
        {
            var repository = ZKWeb.Application.Ioc.Resolve<IRepository<UserToRole, Guid>>();
            var roleRepository = ZKWeb.Application.Ioc.Resolve<IRepository<Role, Guid>>();
            var newRoles = roleRepository.Query().Where(r => roleIds.Contains(r.Id)).ToList();
            var mtmEntities = newRoles.Select(r => new UserToRole() { From = user, To = r }).ToList();
            repository.BatchDelete(t => t.From.Id == user.Id);
            user.Roles.Clear();
            user.Roles.AddRange(mtmEntities);
            Repository.Save(ref user);
        }

        /// <summary>
        /// 获取所有用户类型
        /// 除了匿名用户类型
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IUserType> GetAllUserTypes()
        {
            return ZKWeb.Application.Ioc.ResolveMany<IUserType>()
                .Where(t => !string.IsNullOrEmpty(t.Type));
        }

        /// <summary>
        /// 获取租户列表对应的超级管理员列表
        /// </summary>
        /// <param name="tenantIds">租户Id列表</param>
        /// <returns></returns>
        public virtual IDictionary<Guid, User> GetSuperAdminsFromTenants(IList<Guid> tenantIds)
        {
            var superAdminTypes = ZKWeb.Application.Ioc.ResolveMany<IUserType>()
                .Where(t => typeof(IAmSuperAdmin).GetTypeInfo().IsAssignableFrom(t.GetType()))
                .Select(t => t.Type).ToList();
            using (UnitOfWork.Scope())
            using (UnitOfWork.DisableFilter(typeof(OwnerTenantFilter)))
            {
                var users = GetMany(q =>
                {
                    return q.Where(u =>
                        tenantIds.Contains(u.OwnerTenantId) &&
                        superAdminTypes.Contains(u.Type));
                });
                var tenantToSuperAdmins = users.GroupBy(u => u.OwnerTenantId)
                    .ToDictionary(x => x.Key, x => x.OrderBy(u => u.CreateTime).First());
                return tenantToSuperAdmins;
            }
        }
    }
}
