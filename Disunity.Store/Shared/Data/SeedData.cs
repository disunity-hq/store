using System;
using System.Linq;

using Disunity.Store.Shared.Data.Hooks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data {

    public class SeedData {

        public SeedData(ApplicationDbContext dbContext, ILogger<SeedData> logger) {


            if (dbContext.Mods.Any()) {
                logger.LogInformation("Database already seeded. Skipping.");
                return; // db has been seeded already, skip
            }

            logger.LogInformation("Seeding database.");
            // TODO put seed data here
            dbContext.SaveChanges();
            logger.LogInformation("Database seeded.");
            
        }

    }

}