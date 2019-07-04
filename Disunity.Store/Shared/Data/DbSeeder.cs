using System;
using System.Linq;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data.Hooks;
using Disunity.Store.Shared.Data.Seeds;
using Disunity.Store.Shared.Startup;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data {

    [AsScoped]
    public class DbSeeder {

        public DbSeeder(UserRoleSeed userRoleSeed,
                        SuperUserSeed superUserSeed) {
            
        }
    }

}