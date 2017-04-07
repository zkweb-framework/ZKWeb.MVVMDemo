using Hangfire;
using Microsoft.AspNetCore.Builder;
using ZKWeb.Hosting.AspNetCore;
using ZKWeb.Server;

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
			// 使用Hangfire中间件
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var config = configManager.WebsiteConfig.ConnectionString;
			GlobalConfiguration.Configuration.UseSqlServerStorage(config);
			app.UseHangfireDashboard();
			app.UseHangfireServer();
			// 使用Swagger中间件
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZKWeb MVVM Demo V1");
			});
			// 使用Mvc中间件
			app.UseMvc();
			// 注册IServiceProvider
			Application.Ioc.RegisterInstance(app.ApplicationServices);
		}
	}
}
