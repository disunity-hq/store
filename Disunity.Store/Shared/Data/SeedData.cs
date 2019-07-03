using System;
using System.Linq;

using Disunity.Store.Shared.Data.Hooks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data {

    public static class SeedData {

        public static void Initialize(IServiceProvider serviceProvider) {
            var dbOptions = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            var hookManager = serviceProvider.GetRequiredService<HookManagerContainer>();
            var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            using (var context = new ApplicationDbContext(dbOptions, hookManager, logger)) {
                // check if db has rows
                if (context.Mods.Any()) {
                    return; // db has been seeded already, skip
                }

                // TODO put seed data here
                context.SaveChanges();
            }
        }

    }

}