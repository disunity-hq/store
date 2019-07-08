using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SmartBreadcrumbs.Extensions;


namespace Disunity.Store.Code.Startup.Binders {

    public class RoutingBinder : IStartupBinder {

        public static void Bind(IServiceCollection services, IConfiguration config) {
            services.AddRouting(options => options.LowercaseUrls = true);
//            services.AddBreadcrumbs(Assembly.GetEntryAssembly());
        }

    }

}