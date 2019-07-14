using System.Collections.Generic;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

using TopoSort;


namespace Disunity.Store.Shared.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(TargetSeed), typeof(UserRoleSeed))]
    public class ModSeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModSeed> _logger;
        private readonly IHostingEnvironment _env;

        public ModSeed(ApplicationDbContext context, ILogger<ModSeed> logger, IHostingEnvironment env) {
            _context = context;
            _logger = logger;
            _env = env;

        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !_context.Mods.Any();
        }

        public async Task Seed() {
            var targets = await _context.Targets.ToListAsync();

            foreach (var target in targets) {
                Org testOrg = new Org();

                for (var i = 0; i < 10; i++) {
                    if (i % 3 == 0) {
                        testOrg = new Org() {
                            Slug = $"test_org_{target.Slug} {i / 3}",
                            DisplayName = $"Test Org {target.DisplayName} {i / 3}"
                        };
                    }

                    var modVersion = new ModVersion() {
                        Description = "This is a mod!",
                        Readme = "# Markdown!",
                        DisplayName = $"test-org-mod-{i}",
                        FileUrl = "",
                        IconUrl = "/assets/logoc_512x512.png",
                        VersionNumber = "",
                        WebsiteUrl = "",
                        TargetCompatibilities = new List<ModTargetCompatibility>()
                            {new ModTargetCompatibility() {Target = target}}
                    };

                    var mod = new Mod() {
                        Owner = testOrg,
                        Slug = $"test_org-mod-{i}",
                        Versions = new List<ModVersion>() {modVersion},
                        DisplayName = $"test_org-mod-{i}"
                    };

                    _context.Mods.Add(mod);
                }
            }


            await _context.SaveChangesAsync();

        }

    }

}