namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Domain.Entities.Interfaces {
	/// <summary>
	/// 标记用户是匿名用户
	/// 这个应该用于表示用户未登录，不存在于数据库中
	/// </summary>
	public interface IAmAnonymouseUser : IUserType { }
}
