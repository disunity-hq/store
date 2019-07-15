using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data.Factories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.Logging;

using TopoSort;


namespace Disunity.Store.Shared.Data.Seeds {

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
            return _env.IsDevelopment() && !_context.Mods.Any();
        }

        public async Task Seed() {
            var targets = await _context.Targets.ToListAsync();

            var random = new Random();

            foreach (var target in targets) {
                Org testOrg = new Org();

                for (var i = 0; i < 10; i++) {
                    if (i % 3 == 0) {
                        testOrg = new Org() {
                            Slug = $"test_org_{target.Slug} {i / 3}",
                            DisplayName = $"Test Org {target.DisplayName} {i / 3}"
                        };
                    }

                    var version = new VersionNumber(random.Next(3), random.Next(3), random.Next(3));
//
//                    var attachedVersion = await _context.VersionNumbers.FirstOrDefaultAsync(
//                        v => v.Major == version.Major &&
//                             v.Minor == version.Minor &&
//                             v.Patch == version.Patch);
//
//                    if (attachedVersion == null) {
//                        attachedVersion = version;
//
//                        _logger.LogInformation(
//                            $"Creating new VersionNumber {attachedVersion.Major}.{attachedVersion.Minor}.{attachedVersion.Patch}");
//
//                        _context.VersionNumbers.Add(attachedVersion);
//                        await _context.SaveChangesAsync();
//
//
//                    }

                    var attachedVersion = await _versionNumberFactory.FindOrCreateVersionNumber(version);

                    var modVersion = new ModVersion() {
                        Description = "This is a mod!",
                        Readme = "# Markdown!",
                        DisplayName = $"test-org-mod-{i}",
                        FileUrl = "",
                        IconUrl = "/assets/logoc_512x512.png",
                        VersionNumber = attachedVersion,
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