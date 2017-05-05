using System;
using System.Collections.Generic;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using Newtonsoft.Json;
using ZKWebStandard.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.MultiTenant.src.Domain.Entities;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    [ExportMany]
    public class User :
        IEntity<Guid>,
        IHaveCreateTime,
        IHaveUpdateTime,
        IHaveDeleted,
        IHaveOwnerTenant,
        IEntityMappingProvider<User>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string Username { get; set; }
        /// <summary>
        /// 密码信息，json
        /// </summary>
        public virtual string PasswordJson { get; set; }
        /// <summary>
        /// 所属的租户
        /// </summary>
        public virtual Tenant OwnerTenant { get; set; }
        public virtual Guid OwnerTenantId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }
        /// <summary>
        /// 用户数据的Json
        /// </summary>
        public virtual string ItemsJson { get; set; }
        private IDictionary<string, object> _items;
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public virtual bool Deleted { get; set; }
        /// <summary>
        /// 关联的角色
        /// </summary>
        public virtual IList<UserToRole> Roles { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public User()
        {
            Roles = new List<UserToRole>();
        }

        /// <summary>
        /// 显示用户名
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Username;
        }

        /// <summary>
        /// 设置用户数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (_items == null)
                {
                    _items = string.IsNullOrEmpty(ItemsJson) ?
                        new Dictionary<string, object>() :
                        JsonConvert.DeserializeObject<IDictionary<string, object>>(ItemsJson);
                }
                return _items.GetOrDefault(key);
            }
            set
            {
                if (_items == null)
                {
                    _items = string.IsNullOrEmpty(ItemsJson) ?
                        new Dictionary<string, object>() :
                        JsonConvert.DeserializeObject<IDictionary<string, object>>(ItemsJson);
                }
                if (value == null)
                {
                    _items.Remove(key);
                }
                else
                {
                    _items[key] = value;
                }
                ItemsJson = JsonConvert.SerializeObject(_items);
            }
        }

        /// <summary>
        /// 获取密码信息
        /// </summary>
        /// <returns></returns>
        public PasswordInfo GetPasswordInfo()
        {
            return string.IsNullOrEmpty(PasswordJson) ?
                new PasswordInfo() :
                JsonConvert.DeserializeObject<PasswordInfo>(PasswordJson);
        }

        /// <summary>
        /// 设置密码信息
        /// </summary>
        /// <param name="passwordInfo">密码信息</param>
        public void SetPasswordInfo(PasswordInfo passwordInfo)
        {
            PasswordJson = JsonConvert.SerializeObject(passwordInfo);
        }

        /// <summary>
        /// 配置数据库结构
        /// </summary>
        public virtual void Configure(IEntityMappingBuilder<User> builder)
        {
            builder.Id(u => u.Id);
            builder.Map(u => u.Type, new EntityMappingOptions() { Index = "Idx_User_Type" });
            builder.Map(u => u.Username, new EntityMappingOptions() { Length = 255 });
            builder.Map(u => u.PasswordJson);
            builder.References(u => u.OwnerTenant, new EntityMappingOptions() { Nullable = false });
            builder.Map(u => u.CreateTime, new EntityMappingOptions() { Index = "Idx_User_CreateTime" });
            builder.Map(u => u.UpdateTime);
            builder.Map(u => u.ItemsJson);
            builder.Map(u => u.Remark);
            builder.Map(u => u.Deleted);
            builder.HasMany(r => r.Roles);
        }
    }
}
