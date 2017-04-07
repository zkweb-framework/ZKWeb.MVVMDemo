using Hangfire;
using System;
using System.FastReflection;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ZKWeb.Web;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.ScheduledTasks.Bases {
	/// <summary>
	/// 计划任务的基础类
	/// 继承这个类的类必须要有静态的Execute函数
	/// </summary>
	public abstract class ScheduledTaskBase<T> : IWebsiteStartHandler {
		/// <summary>
		/// 任务Id
		/// </summary>
		public abstract string JobId { get; }
		/// <summary>
		/// 间隔表达式
		/// </summary>
		public abstract string CronExpression { get; }

		/// <summary>
		/// 等待Hangfire配置完成后添加任务
		/// 这里使用了一个奇技淫巧
		/// 通过构建静态函数的表达式传给Hangfire可以避免Hangfire使用依赖注入
		/// Hangfire一旦使用依赖注入任何方式都不能避免，只能乖乖的使用微软的容器，并且任何指定JobActivator的方法都不会起效
		/// 这是唯一不使用微软的容器也能执行自己的任务的办法
		/// </summary>
		public void OnWebsiteStart() {
			var executeMethod = typeof(T).FastGetMethod(
				"Execute", BindingFlags.Static | BindingFlags.Public);
			if (executeMethod == null ||
				executeMethod.ReturnType != typeof(void) ||
				executeMethod.GetParameters().Length != 0) {
				throw new ArgumentException(
					$"No valid 'Execute' method in type '{typeof(T)}', " +
					"it should take no parameters and return void");
			}
			Task.Factory.StartNew(async () => {
				while (true) {
					await Task.Delay(100);
					if (JobStorage.Current != null) {
						var expr = Expression.Lambda<Action>(Expression.Call(executeMethod));
						RecurringJob.AddOrUpdate(JobId, expr, CronExpression);
						break;
					}
				}
			});
		}
	}
}
