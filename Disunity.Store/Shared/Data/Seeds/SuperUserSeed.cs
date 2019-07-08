using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Startup;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data.Seeds {

    [AsScoped]
    public class SuperUserSeed : ISeeder {

        private readonly ILogger<SuperUserSeed> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public SuperUserSeed(IConfiguration config,
                             ILogger<SuperUserSeed> logger,
                             UserManager<UserIdentity> userManager,
                             ApplicationDbContext dbContext) {


            _logger = logger;
            _config = config;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public bool ShouldSeed() {
            return _dbContext.Users.Any();
        }

        public async Task Seed() {
            var password = _config["AdminUser:Password"];
            var emailAddress = _config["AdminUser:Email"];

            if (emailAddress == null || password == null) {
                _logger.LogWarning("Skipping creating super user as user was missing email or password");
                return;
            }

            if (await _userManager.FindByEmailAsync(emailAddress) != null) {
                _logger.LogWarning($"Skipping creating super user as email is already taken: {emailAddress}");
                return;
            }

            var superuser = new UserIdentity {
                UserName = _config.GetValue("AdminUser:Name", emailAddress),
                Email = emailAddress
            };

            var createSuperUser = await _userManager.CreateAsync(superuser, password);

            if (!createSuperUser.Succeeded) {
                _logger.LogWarning($"Failed to create admin user: {emailAddress}");
                return;
            }

            _logger.LogInformation($"Successfully created admin user: {superuser.Email}");

            var addedRole = await _userManager.AddToRoleAsync(superuser, UserRoles.Admin.ToString());

            if (addedRole.Succeeded) {
                _logger.LogInformation($"Successfully set admin role: {superuser.Email}");
            }

        }

    }

}