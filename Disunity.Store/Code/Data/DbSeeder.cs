using System.Collections.Generic;

using Disunity.Store.Code.Data.Seeds;
using Disunity.Store.Code.Startup;


namespace Disunity.Store.Code.Data {

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