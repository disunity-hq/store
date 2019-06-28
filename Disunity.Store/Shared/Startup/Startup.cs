using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Startup {

    public class Startup {

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; protected set; }

        public Startup(ILoggerFactory logFactory, IConfiguration configuration) {
            Logger = logFactory.CreateLogger<Startup>();
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            ServicesStartup.ConfigureServices(services, Configuration, Logger);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider) {
            AppStartup.Startup(Configuration, app, env, serviceProvider);
        }

    }

}