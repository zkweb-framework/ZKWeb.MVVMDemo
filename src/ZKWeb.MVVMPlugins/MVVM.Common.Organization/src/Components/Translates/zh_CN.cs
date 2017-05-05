using System.Collections.Generic;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.Translates.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Translates
{
    /// <summary>
    /// 中文翻译
    /// </summary>
    [ExportMany]
    public class zh_CN : DictionaryTranslationProviderBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public zh_CN()
        {
            Codes = new HashSet<string>() { "zh-CN" };
            Translates = new Dictionary<string, string>()
            {
                { "Admin Login", "管理员登录" },
                { "Tenant", "租户" },
                { "Username", "用户名" },
                { "Password", "密码" },
                { "ConfirmPassword", "确认密码" },
                { "Captcha", "验证码" },
                { "Default admin account is admin, password is 123456, please change it after login immediately",
                    "默认管理员账号: admin, 密码: 123456, 登录后请尽快修改密码" },
                { "Incorrect username or password", "用户名或密码错误" },
                { "Incorrect captcha", "验证码错误" },
                { "Action require user to be '{0}', and have privileges '{1}'", "操作要求用户是'{0}'，且拥有权限'{1}'" },
                { "Action require user to be '{0}'", "操作要求用户是'{0}'" },
                { "Action require user under master tenant", "操作要求用户属于主租户" },
                { "Role", "角色" },
                { "Roles", "角色" },
                { "User", "用户" },
                { "UserType", "用户类型" },
                { "Admin", "管理员" },
                { "SuperAdmin", "超级管理员" },
                { "Action require the ownership of {0}: {1}", "操作需要拥有以下{0}的所有权: {1}" },
                { "Action require the tenant ownership of {0}: {1}", "操作需要拥有以下{0}的租户所有权: {1}" },
                { "Admin Index", "后台首页" },
                { "System Manage", "系统管理" },
                { "Tenant Manage", "租户管理" },
                { "User Manage", "用户管理" },
                { "Role Manage", "角色管理" },
                { "System Settings", "系统设置" },
                { "Website Settings", "网站设置" },
                { "404 Not Found", "404 找不到页面" },
                { "Go Back", "返回上一页" },
                { "About Website", "关于网站" },
                { "Clear Cache", "清理缓存" },
                { "About Me", "关于我" },
                { "Logout", "退出登录" },
                { "Name", "名称" },
                { "IsMasterTenant", "是否主租户" },
                { "CreateTime", "创建时间" },
                { "UpdateTime", "更新时间" },
                { "Please Select", "请选择" },
                { "Yes", "是" },
                { "No", "否" },
                { "Confirmation", "请确认" },
                { "Are you sure to remove '{0}'?", "确认删除'{0}'?" },
                { "Edit Tenant", "编辑租户" },
                { "Edit User", "编辑用户" },
                { "Edit Role", "编辑角色" },
                { "Remark", "备注" },
                { "You can't delete master tenant", "不能删除主租户" },
                { "Saved Successfully", "保存成功" },
                { "Deleted Successfully", "删除成功" },
                { "OwnerTenantName", "租户名" },
                { "Privileges", "权限" },
                { "Please provider a password for new user", "请提供新用户的密码" },
                { "Type", "类型" },
                { "SuperAdminName", "超级管理员名称" },
                { "SuperAdminPassword", "超级管理员密码" },
                { "SuperAdminConfirmPassword", "超级管理员确认密码" },
                { "Confirm password not matched with password", "确认密码和密码不一致" },
                { "Username has been taken", "用户名已被使用" },
                { "Clear Cache Successfully", "清理缓存成功" },
                { "Language", "语言" },
                { "Timezone", "时区" },
                { "Api Url Base","Api基础地址" },
                { "ZKWeb Version", "ZKWeb版本" },
                { "ZKWeb Full Version", "ZKWeb完整版本" },
                { "Memory Usage", "内存占用" },
                { "Plugin List", "插件列表" },
                { "DirectoryName", "目录名称" },
                { "PluginName", "插件名称" },
                { "Version", "版本" },
                { "FullVersion", "完整版本" },
                { "Description", "描述" },
                { "Base Information", "基本信息" },
                { "Change Password", "修改密码" },
                { "Change Avatar", "修改头像" },
                { "OldPassword", "原密码" },
                { "NewPassword", "新密码" },
                { "ConfirmNewPassword", "确认新密码" },
                { "Avatar", "头像" },
                { "Incorrect old password", "原密码不正确" },
                { "Change Password Successfully", "修改密码成功" },
                { "Upload Avatar Successfully", "上传头像成功" },
                { "Settings", "设置" },
                { "WebsiteSettings", "网站设置" },
                { "WebsiteName", "网站名称" },
                { "ScheduledTask", "定时任务" },
                { "ScheduledTaskLog", "定时任务记录" },
                { "Scheduled Tasks", "定时任务" },
                { "Scheduled Tasks Log", "定时任务记录" },
                { "Executed Time", "执行时间" },
                { "First Executed Time", "首次执行时间" },
                { "Last Executed Time", "上次执行时间" },
                { "ErrorMessage", "错误信息" },
                { "IsSuccess", "是否成功" },
                { "Switch Language", "切换语言" },
                { "Switch Timezone", "切换时区" }
            };
        }
    }
}
