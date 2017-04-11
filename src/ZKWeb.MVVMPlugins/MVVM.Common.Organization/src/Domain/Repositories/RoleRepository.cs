using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Repositories {
	/// <summary>
	/// 用户角色的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RoleRepository : RepositoryBase<Role, Guid> {
		/// <summary>
		/// 查询时包含关联数据
		/// </summary>
		/// <returns></returns>
		public override IQueryable<Role> Query() {
			return base.Query().Include(r => r.Users);
		}
	}
}
