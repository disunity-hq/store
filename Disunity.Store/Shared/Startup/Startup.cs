using System.Collections.Generic;

using Disunity.Store.Shared.Startup.Binders;
using Disunity.Store.Shared.Startup.Services;

using EFCoreHooks.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup {

    public class Startup {

        private IConfiguration _config;

        public Startup(ILoggerFactory logFactory, IConfiguration config) {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.ConfigureAttributes();
            services.ConfigureAuthentication(_config);
            services.ConfigureAuthorization();
            services.ConfigureCookies();
            services.ConfigureDatabase(_config);
            services.ConfigureIdentity();
            services.ConfigureMvc();
            services.ConfigureRouting();
            services.ConfigureDbHooks();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IEnumerable<IStartupService> startupServices) {
            foreach (var startupService in startupServices) {
                logger.LogInformation($"Starting service: {startupService.GetType().Name}");
                startupService.Startup(app);
            }
        }

    }

}