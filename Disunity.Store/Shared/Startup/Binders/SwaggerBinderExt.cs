using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;


namespace Disunity.Store.Shared.Startup.Binders {

    public static class SwaggerBinderExt {

        public static void ConfigureSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info() {Title = "Disunity API", Version = "v1"}); });
        }

    }

}