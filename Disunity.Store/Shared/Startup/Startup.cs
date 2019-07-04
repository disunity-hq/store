using System.Collections.Generic;

using Disunity.Store.Shared.Startup.Binders;
using Disunity.Store.Shared.Startup.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup {

    public class Startup {

        private ILogger _logger { get; set; }
        private IEnumerable<IStartupBinder> _binders { get; }


        public Startup(ILoggerFactory logFactory, IConfiguration config, IEnumerable<IStartupBinder> binders) {
            _logger = logFactory.CreateLogger<Startup>();
            _binders = binders;
        }

        public void ConfigureServices(IServiceCollection services) {
            foreach (var startupBinder in _binders) {
                _logger.LogInformation($"Starting binder: {startupBinder.GetType().Name}");
                startupBinder.Bind(services);
            }
        }

        public void Configure(IApplicationBuilder app, IEnumerable<IStartupService> startupServices) {
            foreach (var startupService in startupServices) {
                _logger.LogInformation($"Starting service: {startupService.GetType().Name}");
                startupService.Startup(app);
            }
        }

    }

}