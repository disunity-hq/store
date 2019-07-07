using Disunity.Store.Shared.Startup.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Disunity.Store.Shared.Startup.Binders {

    public class MvcBinder : IStartupBinder {

        public void Bind(IServiceCollection services) {
            services.AddMvc(options => { options.Filters.Add(new BreadcrumbFilter()); })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddNewtonsoftJson(options => {
                        // we need this
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddApiVersioning(options => { options.Conventions.Add(new VersionByNamespaceConvention()); });

            services.AddAntiforgery(options => { options.HeaderName = "xsrf-token"; });
        }

    }

}