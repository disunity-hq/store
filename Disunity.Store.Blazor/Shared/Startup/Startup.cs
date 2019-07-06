using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Disunity.Store.Shared.Startup {

    public class Startup {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            ServicesStartup.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider) {
            AppStartup.Startup(Configuration, app, env, serviceProvider);
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
			app.AddComponent<App>("app");
        }
    }

}