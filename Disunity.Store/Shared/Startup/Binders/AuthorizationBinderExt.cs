using System.Net;

using Disunity.Store.Startup.Policies;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;


namespace Disunity.Store.Startup.Binders {

    public static class AuthorizationBinderExt {

        public static void ConfigureAuthorization(this IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("IsMember", policy => policy.AddRequirements(new OrgMemberRequirement()));
            });
            
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

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