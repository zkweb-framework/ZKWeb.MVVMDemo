using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace ZKWeb.MVVMDemo.AspNetCore {
	/// <summary>
	/// Asp.Net Core Startup Class
	/// </summary>
	public class Startup : ZKWeb.Hosting.AspNetCore.StartupBase {
		/// <summary>
		/// 配置中间件
		/// </summary>
		/// <param name="app"></param>
		protected override void ConfigureMiddlewares(IApplicationBuilder app) {
			// 使用错误提示页面
			var env = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseStatusCodePages();
			}
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
