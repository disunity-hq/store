using System;
using System.Linq;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Startup;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data.Seeds {
    
    [AsScoped]
    public class UserRoleSeed {

        private readonly ILogger<UserRoleSeed> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleSeed(ApplicationDbContext dbContext,
                             RoleManager<IdentityRole> roleManager,
                             ILogger<UserRoleSeed> logger) {

            _logger = logger;
            _roleManager = roleManager;

            if (dbContext.UserRoles.Any()) {
                logger.LogInformation("Skipping.");
            } else {
                logger.LogInformation("Seeding.");
                Seed();
                logger.LogInformation("Seeded.");
            }

        }

        private async void Seed() {
            var roleNames = Enum.GetNames(typeof(UserRoles));

            foreach (var roleName in roleNames) {

                if (await _roleManager.RoleExistsAsync(roleName)) {
                    continue;
                }

                await _roleManager.CreateAsync(new IdentityRole(roleName));
                _logger.LogInformation($"Created role: {roleName}");
            }
            
        }

    }

}