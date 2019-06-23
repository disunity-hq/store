using System;
using System.Linq;
using Disunity.Store.Areas.Mods.Models;
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

                context.Mods.Add(new Mod {Name = "Foobar"});

                // TODO put seed data here
                context.SaveChanges();
            }
        }

    }

}