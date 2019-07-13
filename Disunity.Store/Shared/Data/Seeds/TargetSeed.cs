using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Startup;


namespace Disunity.Store.Shared.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    public class TargetSeed : ISeeder {

        private readonly ApplicationDbContext _context;

        public TargetSeed(ApplicationDbContext context) {
            _context = context;
        }

        public bool ShouldSeed() {
            return !_context.Targets.Any();
        }

        public Task Seed() {
            var targetVersion = new TargetVersion() {
                Description = "Foo Bar the Game",
                Hash = "0123456789abcdef",
                DisplayName = "Foo Bar",
                IconUrl = "",
                WebsiteUrl = "",
                VersionNumber = "1",
                DisunityCompatibility = new TargetVersionCompatibility() {
                    MaxCompatibleVersion = new UnityVersion() {Version = "2018"},
                    MinCompatibleVersion = new UnityVersion() {Version = "2015"}
                }
            };

            _context.Targets.Add(new Target() {
                Slug = "/foobar",
                DisplayName = "Foo bar",
                Versions = new List<TargetVersion>() {
                    targetVersion
                }
            });

            return _context.SaveChangesAsync();
        }

    }

}