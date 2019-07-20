using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Artifacts;
using Disunity.Store.Data;
using Disunity.Store.Extensions;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities.Factories {
    
    public class ModVersionFactory : IModVersionFactory {

        private readonly ApplicationDbContext _context;
        private readonly IVersionNumberFactory _versionNumberFactory;

        public ModVersionFactory(ApplicationDbContext context, IVersionNumberFactory versionNumberFactory) {
            _context = context;
            _versionNumberFactory = versionNumberFactory;

        }

        [Factory]
        public static Func<Archive, Task<ModVersion>> FromArchiveAsync(IServiceProvider services) {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var versionFactory = services.GetRequiredService<IVersionNumberFactory>();

            var factory = new ModVersionFactory(context, versionFactory);

            return factory.FromArchiveAsync;
        }

        public async Task<ModVersion> FromArchiveAsync(Archive archive) {
            var manifest = archive.Manifest;

            var modVersion = new ModVersion() {
                Description = manifest.Description,
                Readme = archive.Readme,
                DisplayName = manifest.DisplayName,
                FileId = "",
                IconUrl = "",
                WebsiteUrl = manifest.URL,
                VersionNumber = await _versionNumberFactory.FindOrCreateVersionNumber(manifest.Version)
            };

            modVersion.ModDependencies = manifest.Dependencies
                                                 .Select(DependencyDictToModDependency(
                                                             modVersion, ModDependencyType.Dependency))
                                                 .Concat(manifest.OptionalDependencies.Select(
                                                             DependencyDictToModDependency(
                                                                 modVersion,
                                                                 ModDependencyType.OptionalDependency)))
                                                 .Concat(manifest.Incompatibilities.Select(
                                                             DependencyDictToModDependency(
                                                                 modVersion, ModDependencyType.Incompatible)))
                                                 .ToList();

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
