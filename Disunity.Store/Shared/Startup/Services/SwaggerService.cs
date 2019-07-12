using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup.Services {

    [AsSingleton(typeof(IStartupService))]
    public class SwaggerService : IStartupService {

        public void Startup(IApplicationBuilder app) {
            app.UseSwagger(c => { c.RouteTemplate = "/api/docs/{documentName}/swagger.json"; });

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/api/docs/v1/swagger.json", "Disunity API v1");
                c.RoutePrefix = "api/docs";
            });
        }

    }

}