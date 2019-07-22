using Disunity.Store.RouteConstraints;

using Microsoft.Extensions.DependencyInjection;

using SmartBreadcrumbs.Extensions;


namespace Disunity.Store.Startup.Binders {

    public static class RoutingBinderExt {

        public static void ConfigureRouting(this IServiceCollection services) {
            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.ConstraintMap.Add("slug", typeof(SlugConstraint));
                options.ConstraintMap.Add("semver", typeof(SemverConstraint));
            });

            var assembly = typeof(RoutingBinderExt).Assembly;
            services.AddBreadcrumbs(assembly);
        }

    }

}