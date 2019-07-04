using System.Collections.Generic;

using Disunity.Store.Shared.Startup.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup {

    public class Startup {

        public Startup(ILoggerFactory logFactory, IConfiguration config) {
            _logger = logFactory.CreateLogger<Startup>();
            _config = config;
        }

        private IConfiguration _config { get; }
        private ILogger _logger { get; set; }

        public void ConfigureServices(IServiceCollection services) {
            ServicesStartup.ConfigureServices(services, _config, _logger);
        }

        public void Configure(IApplicationBuilder app, IEnumerable<IStartupService> startupServices) {
            foreach (var startupService in startupServices) {
                _logger.LogWarning($"Starting service: {startupService.GetType().Name}");
                startupService.Startup(app);
            }
        }

    }

}