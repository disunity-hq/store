using System;
using System.IO;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Shared.Data;
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
        }

        public static void ConfigureMvc(IServiceCollection services) {
            services.AddMvc().AddRazorPagesOptions(options => {
                options.Conventions.AuthorizeAreaFolder("Admin", "/", "IsAdmin");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options => { options.Conventions.Add(new VersionByNamespaceConvention()); });
        }

        public static void ConfigureAuthorization(IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
            });
        }

        public static void ConfigureAntiforgery(IServiceCollection services) {
            services.AddAntiforgery(options => { options.HeaderName = "xsrf-token"; });
        }

        public static void ConfigureRouting(IServiceCollection services) {
            services.AddRouting(options => { options.LowercaseUrls = true; });
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
            ConfigureMvc(services);
            ConfigureAuthorization(services);
            ConfigureAuthentication(services, configuration);
            ConfigureAntiforgery(services);
            ConfigureRouting(services);
        }

    }

}