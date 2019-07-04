using Disunity.Store.Shared.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Shared.Startup.Binders {

    public class DbContextBinder : IStartupBinder {

        private readonly IConfiguration _config;

        public DbContextBinder(IConfiguration config) {
            _config = config;
        }

        public void Bind(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
            });

        }

    }

}