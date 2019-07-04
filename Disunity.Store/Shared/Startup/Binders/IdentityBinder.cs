using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Shared.Startup.Binders {

    public class IdentityBinder : IStartupBinder {

        public void Bind(IServiceCollection services) {
            services.AddDefaultIdentity<UserIdentity>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
        }

    }

}