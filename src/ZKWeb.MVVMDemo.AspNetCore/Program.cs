using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using ZKWeb.MVVMDemo.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
			try {
				var host = new WebHostBuilder()
					.ConfigureServices(s => {
						// 添加Mvc组件
						s.AddMvcCore().AddApiExplorer();
						// 添加Hangfire组件
						s.AddHangfire(_ => { });
						// 添加Swgger组件，使用自定义的Api列表提供器
						s.Replace(new ServiceDescriptor(
							typeof(IApiDescriptionGroupCollectionProvider),
							new ZKWebSwaggerApiProvider()));
						s.AddSwaggerGen(c => {
							c.OperationFilter<ZKWebSwaggerOperationFilter>();
							c.SchemaFilter<ZKWebSwaggerSchemaFilter>();
							c.DocInclusionPredicate((a, b) => true);
							c.SwaggerDoc("v1", new Info() { Title = "ZKWeb MVVM Demo", Version = "V1" });
						});
					})
					.UseKestrel()
					.UseIISIntegration()
					.UseStartup<Startup>()
					.Build();
				host.Run();
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}
	}
}
