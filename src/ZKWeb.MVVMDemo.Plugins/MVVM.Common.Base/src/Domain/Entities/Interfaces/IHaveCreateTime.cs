using System;
using ZKWeb.Database;

namespace ZKWeb.MVVMDemo.Plugins.MVVM.Common.Base.src.Domain.Entities.Interfaces {
	/// <summary>
	/// 包含创建时间的接口
	/// </summary>
	public interface IHaveCreateTime : IEntity {
		/// <summary>
		/// 创建时间
		/// </summary>
		DateTime CreateTime { get; set; }
	}
}
