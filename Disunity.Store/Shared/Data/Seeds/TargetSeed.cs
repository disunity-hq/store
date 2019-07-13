using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Startup;

using Microsoft.AspNetCore.Hosting;


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

            var target = new Target() {
                Slug = "/foobar",
                DisplayName = "Foo bar",
                Versions = new List<TargetVersion>() {
                    targetVersion
                }
            };

            var modVersion = new ModVersion() {
                Description = "This is a mod for foobar",
                Readme = "# Markdown!",
                DisplayName = "test-org-mod",
                FileUrl = "",
                IconUrl = "",
                VersionNumber = "",
                WebsiteUrl = "",
                TargetCompatibilities = new List<ModTargetCompatibility>()
                    {new ModTargetCompatibility() {Target = target}}
            };

            var mod = new Mod() {
                Owner = new Org() {Slug = "/test_org", DisplayName = "Test Org"},
                Slug = "/test_org-mod",
                Versions = new List<ModVersion>() {modVersion},
                DisplayName = "asdf"
            };

            _context.Add(mod);

            return _context.SaveChangesAsync();
        }

    }

}