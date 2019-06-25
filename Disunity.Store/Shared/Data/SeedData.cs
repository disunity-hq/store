using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disunity.Store.Shared.Data {

    public static class SeedData {

        public static void Initialize(IServiceProvider serviceProvider) {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())) {
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