using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Artifacts;
using Disunity.Store.Data;
using Disunity.Store.Exceptions;
using Disunity.Store.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities.Factories {

    public class ModVersionFactory : IModVersionFactory {

        private readonly ApplicationDbContext _context;
        private readonly IVersionNumberFactory _versionNumberFactory;

        public ModVersionFactory(ApplicationDbContext context, IVersionNumberFactory versionNumberFactory) {
            _context = context;
            _versionNumberFactory = versionNumberFactory;

        }

        [ScopedFactory]
        public static Func<Archive, Org, Task<ModVersion>> FromArchiveAsync(IServiceProvider services) {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var versionFactory = services.GetRequiredService<IVersionNumberFactory>();

            var factory = new ModVersionFactory(context, versionFactory);

            return (archive, org) => factory.FromArchiveAsync(archive);
        }

        public async Task<ModVersion> FromArchiveAsync(Archive archive) {
            var manifest = archive.Manifest;

            var mod = await _context.Mods.FirstOrDefaultAsync(m => m.Slug == manifest.ModID);
            var owner = await _context.Orgs.FirstOrDefaultAsync(o => o.Slug == manifest.OrgID);

            if (mod == null) {
                mod = new Mod() {
                    Owner = owner,
                    Slug = manifest.ModID,
                    DisplayName = manifest.DisplayName
                };

                _context.Attach(mod);
            }

            var modVersion = new ModVersion() {
                Mod = mod,
                Description = manifest.Description,
                Readme = archive.Readme,
                DisplayName = manifest.DisplayName,
                FileId = "",
                IconUrl = "",
                WebsiteUrl = manifest.URL,
                VersionNumber = await _versionNumberFactory.FindOrCreateVersionNumber(manifest.Version),
            };
            
            var requiredDeps = manifest.Dependencies
                                       .Select(DependencyDictToModDependency(
                                                   modVersion, ModDependencyType.Dependency));

            var optionalDeps = manifest.OptionalDependencies.Select(
                DependencyDictToModDependency(
                    modVersion, ModDependencyType.OptionalDependency));

            var incompatible = manifest.Incompatibilities.Select(
                DependencyDictToModDependency(
                    modVersion, ModDependencyType.Incompatible));

            modVersion.ModDependencies = requiredDeps.Concat(optionalDeps).Concat(incompatible).ToList();

            _context.Attach(modVersion);

            return modVersion;
        }

        private Func<KeyValuePair<string, VersionRange>, ModDependency> DependencyDictToModDependency(
            ModVersion modVersion, ModDependencyType dependencyType) {
            return d => new ModDependency() {
                Dependant = modVersion,
                Dependency = _context.Mods.FindModByDepString(d.Key).Result,
                DependencyType = dependencyType,
                MinVersion = _context
                             .ModVersions
                             .FindModVersionByDepString(d.Key, d.Value.MinVersion)
                             .Result,
                MaxVersion = _context
                             .ModVersions
                             .FindModVersionByDepString(d.Key, d.Value.MaxVersion)
                             .Result
            };
        }

    }

}