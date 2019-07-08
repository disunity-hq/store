using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Disunity.Store.Code.Startup.Binders;
using Disunity.Store.Code.Startup.Services;
using Disunity.Store.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Code.Startup {

    public class Startup {

        private IConfiguration _config { get; }

        public Startup(IConfiguration config) {
            _config = config;
            Console.WriteLine("We are starting up...");
        }

        public void ConfigureServices(IServiceCollection services) {
            Console.WriteLine("Configuring services...");

            var binders = Assembly
                          .GetEntryAssembly()
                          ?.GetTypes()
                          .Where(t => t.Name.EndsWith("Binder") && t.IsClass);
            
            Console.WriteLine($"There are {binders.Count()} binders...");

            foreach (var binder in binders) {
                Console.WriteLine($"Found binder: {binder.Name}");
                var bindMethod = binder.GetMethod("Bind", BindingFlags.Public | BindingFlags.Static);
                bindMethod.Invoke(null, new object[] {services, _config});
            }

            services.AddSingleton<WeatherForecastService>();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IEnumerable<IStartupService> startupServices) {
            foreach (var startupService in startupServices) {
                logger.LogInformation($"Starting service: {startupService.GetType().Name}");
                startupService.Startup(app);
            }
        }

    }

}