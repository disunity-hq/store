using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using SmartBreadcrumbs.Extensions;


namespace Disunity.Store.Shared.Startup.Binders {

    public class RoutingBinder : IStartupBinder {

        public void Bind(IServiceCollection services) {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddBreadcrumbs(Assembly.GetEntryAssembly());
        }

    }

}