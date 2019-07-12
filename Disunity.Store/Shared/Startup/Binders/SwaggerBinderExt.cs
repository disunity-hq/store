using Disunity.Store.Shared.Startup.Filters;

using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;


namespace Disunity.Store.Shared.Startup.Binders {

    public static class SwaggerBinderExt {

        public static void ConfigureSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(swagger => { swagger.OperationFilter<SwaggerDefaultValues>(); });
        }

    }

}