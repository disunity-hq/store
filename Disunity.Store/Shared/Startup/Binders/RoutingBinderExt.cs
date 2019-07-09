using System;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using SmartBreadcrumbs.Extensions;


namespace Disunity.Store.Shared.Startup.Binders {

    public static class RoutingBinderExt {

        public static void ConfigureRouting(this IServiceCollection services) {
            services.AddRouting(options => options.LowercaseUrls = true);
            var assembly = typeof(RoutingBinderExt).Assembly;
            services.AddBreadcrumbs(assembly);
        }

    }

}