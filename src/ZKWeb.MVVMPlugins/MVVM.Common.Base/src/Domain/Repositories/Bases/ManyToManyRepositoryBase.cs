using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Bases;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases {
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TFrom"></typeparam>
	/// <typeparam name="TTo"></typeparam>
	/// <typeparam name="TEntity"></typeparam>
	public class ManyToManyRepositoryBase<TFrom, TTo, TEntity> : RepositoryBase<TEntity, Guid>
		where TFrom : class, IEntity
		where TTo : class, IEntity
		where TEntity : ManyToManyEntityBase<TFrom, TTo, TEntity> {
	}
}
