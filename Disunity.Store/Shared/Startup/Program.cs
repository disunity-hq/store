﻿using System.Linq;
using System.Reflection;

using Disunity.Store.Shared.Startup.Binders;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Shared.Startup {

    public class Program {

        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                          .UseWebRoot("../Frontend/dist")
                          .ConfigureServices(di => {
                              Assembly
                                  .GetEntryAssembly()
                                  ?.GetTypes()
                                  .Where(t => t.Name.EndsWith("Binder") && t.IsClass)
                                  .ForEach(t => di.AddSingleton(typeof(IStartupBinder), t));
                          })
                          .ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables(); })
                          .ConfigureLogging(f => f.AddConsole().AddDebug())
                          .UseStartup<Startup>();
        }

    }

}