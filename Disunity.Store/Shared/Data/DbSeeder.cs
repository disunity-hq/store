using System;
using System.Collections.Generic;
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

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Shared.Data {

    [AsScoped]
    public class DbSeeder {

        private readonly IEnumerable<ISeeder> _seeds;
        
        public DbSeeder(IEnumerable<ISeeder> seeds) {
            _seeds = seeds;
        }

        public async void Seed() {
            foreach (var seed in _seeds) {
                if (seed.ShouldSeed()) {
                    await seed.Seed();
                }
            }
        }
    }

}