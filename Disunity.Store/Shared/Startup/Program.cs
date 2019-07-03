using System;

using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup {

    public class Program {

        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                          .UseWebRoot("../Frontend/dist")
                          .ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables(); })
                          .ConfigureLogging(f => f.AddConsole().AddDebug())
                          .UseStartup<Startup>();
        }

    }

}