using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Startup.Filters;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using SmartBreadcrumbs;
using SmartBreadcrumbs.Extensions;


namespace Disunity.Store.Shared.Startup {

    public static class ServicesStartup {

        private static void ConfigureBindings(IServiceCollection services) {
            Binding.ConfigureBindings(services);
        }

        private static void ConfigureFactories(IServiceCollection services) {
            FactoryAttribute.ConfigureFactories(services);
        }

        private static void ConfigureCookiePolicy(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<ApplicationDbContext>(options =>
                                                            options.UseNpgsql(
                                                                configuration
                                                                    .GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<UserIdentity>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<SeedData>();
        }

        public static void ConfigureMvc(IServiceCollection services) {
            services.AddMvc(options => {
                        options.Filters.Add(new BreadcrumbFilter());
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
//                    .AddRazorPagesOptions(options => {
//                        options.Conventions.AuthorizeFolder("/Admin", "IsAdmin");
//                    })
                    
                    .AddJsonOptions(options => {
                        // we need this
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddApiVersioning(options => { options.Conventions.Add(new VersionByNamespaceConvention()); });
        }

        public static void ConfigureAuthorization(IServiceCollection services) {
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

        public static void ConfigureAntiforgery(IServiceCollection services) {
            services.AddAntiforgery(options => { options.HeaderName = "xsrf-token"; });
        }

        public static void ConfigureRouting(IServiceCollection services) {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddBreadcrumbs(typeof(ServicesStartup).Assembly);
        }

        public static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration) {
            var authenticationBuilder = services.AddAuthentication();
            var githubClientId = configuration.GetValue<string>("Auth:Github:ClientId");
            var githubClientSecret = configuration.GetValue<string>("Auth:Github:ClientSecret");
            var discordClientId = configuration.GetValue<string>("Auth:Discord:ClientId");
            var discordClientSecret = configuration.GetValue<string>("Auth:Discord:ClientSecret");

            if (!string.IsNullOrWhiteSpace(githubClientId) && !string.IsNullOrWhiteSpace(githubClientSecret)) {
                authenticationBuilder.AddGitHub(options => {
                    options.ClientId = githubClientId;
                    options.ClientSecret = githubClientSecret;
                });
            }

            if (!string.IsNullOrWhiteSpace(discordClientId) && !string.IsNullOrWhiteSpace(discordClientSecret)) {
                authenticationBuilder.AddDiscord(options => {
                    options.ClientId = discordClientId;
                    options.ClientSecret = discordClientSecret;
                });
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ILogger log) {
            ConfigureBindings(services);
            ConfigureFactories(services);
            ConfigureCookiePolicy(services);
            ConfigureDbContext(services, configuration);
            ConfigureRouting(services);
            ConfigureMvc(services);
            ConfigureAuthorization(services);
            ConfigureAuthentication(services, configuration);
            ConfigureAntiforgery(services);
        }

    }

}