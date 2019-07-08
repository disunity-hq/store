using Disunity.Store.Code.Startup.Filters;

using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Disunity.Store.Code.Startup.Binders {

    public class MvcBinder : IStartupBinder {

        public static void Bind(IServiceCollection services, IConfiguration config) {
            services.AddMvc(options => { options.Filters.Add(new BreadcrumbFilter()); })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddNewtonsoftJson(options => {
                        // we need this
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddRazorPages();
            services.AddServerSideBlazor();
//            services.AddApiVersioning(options => { options.Conventions.Add(new VersionByNamespaceConvention()); });
            services.AddAntiforgery(options => { options.HeaderName = "xsrf-token"; });
        }

    }

}