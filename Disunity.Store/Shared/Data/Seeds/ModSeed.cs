using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;
using Disunity.Store.Entities.Factories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TopoSort;

using Tracery;

using EnumerableExtensions = Microsoft.EntityFrameworkCore.Internal.EnumerableExtensions;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(TargetSeed), typeof(UserRoleSeed))]
    public class ModSeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModSeed> _logger;
        private readonly IHostingEnvironment _env;
        private readonly IVersionNumberFactory _versionNumberFactory;

        public ModSeed(ApplicationDbContext context, ILogger<ModSeed> logger, IHostingEnvironment env,
                       IVersionNumberFactory versionNumberFactory) {
            _context = context;
            _logger = logger;
            _env = env;
            _versionNumberFactory = versionNumberFactory;
        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !EnumerableExtensions.Any(_context.Mods);
        }

        public async Task Seed() {
            var random = new Random();
            var orgs = _context.Orgs.ToList();
            var targets = await _context.Targets.ToListAsync();


            for (var i = 0; i < 45; i++) {
                var org = orgs.PickRandom();
                var target = targets.PickRandom();
                var version = new VersionNumber(random.Next(3), random.Next(3), random.Next(3));
                var attachedVersion = await _versionNumberFactory.FindOrCreateVersionNumber(version);

                var modVersion = new ModVersion() {
                    Description = "This is a mod!",
                    Readme = "# Markdown!",
                    DisplayName = $"test-org-mod-{i}",
                    FileId = "",
                    IconUrl = "/assets/logo_512x512.png",
                    VersionNumber = attachedVersion,
                    WebsiteUrl = "",
                    TargetCompatibilities = new List<ModTargetCompatibility>()
                        {new ModTargetCompatibility() {Target = target}}
                };

                var mod = new Mod() {
                    Owner = org,
                    Slug = $"test_org-mod-{i}",
                    Versions = new List<ModVersion>() {modVersion},
                    DisplayName = $"test_org-mod-{i}"
                };

                _context.Mods.Add(mod);
            }

            await _context.SaveChangesAsync();

        }

    }

}