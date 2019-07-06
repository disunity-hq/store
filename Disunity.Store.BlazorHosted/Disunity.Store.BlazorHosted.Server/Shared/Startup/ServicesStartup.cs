using System.Linq;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Shared.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Disunity.Store.Shared.Startup {

    public static class ServicesStartup {

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
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        public static void ConfigureMvc(IServiceCollection services) {
            services.AddMvc()
	            .AddNewtonsoftJson()
	            .AddRazorPagesOptions(options => {
					options.Conventions.AuthorizeAreaFolder("Admin", "/", "IsAdmin");
				})
	            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			services.AddResponseCompression(opts => {
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" });
			});
		}

        public static void ConfigureAuthorization(IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
	        ConfigureCookiePolicy(services);
            ConfigureDbContext(services, configuration);
            ConfigureMvc(services);
            ConfigureAuthorization(services);
        }

    }

}