using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Repositories {
	/// <summary>
	/// 用户的仓储
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserRepository : RepositoryBase<User, Guid> {
		/// <summary>
		/// 查询时包含关联数据
		/// </summary>
		/// <returns></returns>
		public override IQueryable<User> Query() {
			return base.Query().Include(c => c.Roles);
		}
	}
}
