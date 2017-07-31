using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.Configuration;

namespace ZKWeb.MVVMDemo.AspNetCore
{
    /// <summary>
    /// Asp.Net Core Main Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                var host = new WebHostBuilder()
                    .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("hosting.json", optional: true)
                        .AddCommandLine(args)
                        .Build())
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();
                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Environment.Exit(-1);
            }
        }
    }
}
