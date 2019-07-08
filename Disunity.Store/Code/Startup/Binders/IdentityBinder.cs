using Disunity.Store.Code.Data;
using Disunity.Store.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Code.Startup.Binders {

    public class IdentityBinder : IStartupBinder {

        public static void Bind(IServiceCollection services, IConfiguration config) {
            services.AddDefaultIdentity<UserIdentity>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
        }

    }

}