using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Entities.Extensions {

    public static class ModVersionQueryableExtensions {

        public static Task<ModVersion> FindModVersionByDepString(this IQueryable<ModVersion> modVersions,
                                                                 string depString, string versionString) {
            var segments = depString.Split('/');
            var orgSlug = segments[0];
            var modSlug = segments[1];

            return modVersions
                   .Where(v => v.Mod.Slug == modSlug)
                   .Where(v => v.Mod.Owner.Slug == orgSlug)
                   .SingleAsync(v => v.VersionNumber == versionString);
        }

    }

}