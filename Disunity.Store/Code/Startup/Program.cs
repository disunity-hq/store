using System;
using System.Linq;
using System.Reflection;

using Disunity.Store.Code.Startup.Binders;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Code.Startup {

    public class Program {

        public static void Main(string[] args) {
            Console.WriteLine("Starting up application.");
            var host = CreateHostBuilder(args);
            Console.WriteLine("Building...");
            var build = host.Build();
            Console.WriteLine("Running...");                    
            build.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            Console.WriteLine("Building host...");
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    Console.WriteLine("Currently building app...");
                    webBuilder
                        .UseWebRoot("../Frontend/dist")
                        .ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables(); })
                        .ConfigureLogging(f => f.AddConsole().AddDebug())
                        .UseStartup<Startup>();
                });
        }

    }

}