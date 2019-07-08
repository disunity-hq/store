using Disunity.Store.Code.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Code.Startup.Binders {

    public class DbContextBinder : IStartupBinder {

        public static void Bind(IServiceCollection services, IConfiguration config) {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

        }

    }

}