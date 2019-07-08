using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;


namespace Disunity.Store.Code.Startup.Binders {

    public class AuthorizationBinder : IStartupBinder {

        public static void Bind(IServiceCollection services, IConfiguration config) {
            services.AddAuthorization(options => {
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
            });

            services.ConfigureApplicationCookie(options => {
                options.Events.OnRedirectToLogin = async ctx => {
                    if (ctx.Request.Path.StartsWithSegments("/api") &&
                        ctx.Response.StatusCode == (int) HttpStatusCode.OK) {
                        ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;

                        var body = JsonConvert.SerializeObject(new {
                            type = "Unauthorized",
                            error = "Not authorized.",
                        }, Formatting.Indented);

                        await ctx.Response.WriteAsync(body);
                    } else {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                };
            });
            
        }

    }

}