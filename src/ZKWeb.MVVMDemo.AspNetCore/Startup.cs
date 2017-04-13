using Microsoft.AspNetCore.Builder;
using ZKWeb.Hosting.AspNetCore;

namespace ZKWeb.MVVMDemo.AspNetCore {
	/// <summary>
	/// Asp.Net Core Startup Class
	/// </summary>
	public class Startup : StartupBase {
		/// <summary>
		/// 配置中间件
		/// </summary>
		/// <param name="app"></param>
		protected override void ConfigureMiddlewares(IApplicationBuilder app) {
			// 使用Swagger中间件
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZKWeb MVVM Demo V1");
			});
			// 使用Mvc中间件
			app.UseMvc(routes => {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
			// 注册IServiceProvider
			Application.Ioc.RegisterInstance(app.ApplicationServices);
		}
	}
}
