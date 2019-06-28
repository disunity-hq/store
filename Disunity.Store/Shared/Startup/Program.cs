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
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                try {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate(); // probably don't do this in production
                    SeedData.Initialize(services);
                }
                catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
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