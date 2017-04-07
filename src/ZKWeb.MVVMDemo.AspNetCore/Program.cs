using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using ZKWeb.MVVMDemo.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ZKWeb.MVVMDemo.AspNetCore {
	/// <summary>
	/// Asp.Net Core Main Program
	/// </summary>
	public class Program {
		/// <summary>
		/// Entry Point
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args) {
			var host = new WebHostBuilder()
				.ConfigureServices(s => {
					s.AddSingleton<IApiDescriptionGroupCollectionProvider, ZKWebSwaggerApiProvider>();
					s.AddHangfire(_ => { });
					s.AddSwaggerGen(c =>
						c.SwaggerDoc("v1", new Info() { Title = "ZKWeb MVVM Demo", Version = "V1" }));
				})
				.UseKestrel()
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();
			host.Run();
		}
	}
}
