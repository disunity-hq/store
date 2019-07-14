using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;

using Microsoft.AspNetCore.Hosting;

using TopoSort;


namespace Disunity.Store.Shared.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    public class TargetSeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public TargetSeed(ApplicationDbContext context, IHostingEnvironment env) {
            _context = context;
            _env = env;
        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !_context.Targets.Any();
        }

        public Task Seed() {
            var unity2015 = new UnityVersion() {VersionNumber = (VersionNumber)"2015.1.0"};
            var unity2018 = new UnityVersion() {VersionNumber = (VersionNumber) "2018.0.1"};
            
            for (var i = 0; i < 10; i++) {
                var targetVersion = new TargetVersion() {
                    Description = "Foo Bar the Game",
                    Hash = $"0123456789abcdef-{i}",
                    DisplayName = $"Foo Bar - {i}",
                    IconUrl = "/assets/logo_512x512.png",
                    WebsiteUrl = "",
                    VersionNumber = "1",
                    DisunityCompatibility = new TargetVersionCompatibility() {
                        MinCompatibleVersion = unity2015,
                        MaxCompatibleVersion = unity2018
                    }
                };

                var target = new Target() {
                    Slug = $"foobar-{i}",
                    DisplayName = $"Foo bar - {i}",
                    Versions = new List<TargetVersion>() {
                        targetVersion
                    }
                };

                _context.Targets.Add(target);
            }


            return _context.SaveChangesAsync();
        }

    }

}