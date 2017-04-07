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
		/// ≈‰÷√÷–º‰º˛
		/// </summary>
		/// <param name="app"></param>
		protected override void ConfigureMiddlewares(IApplicationBuilder app) {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var config = configManager.WebsiteConfig.ConnectionString;
			GlobalConfiguration.Configuration.UseSqlServerStorage(config);
			app.UseHangfireDashboard();
			app.UseHangfireServer();
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZKWeb MVVM Demo V1");
			});
		}
	}
}
