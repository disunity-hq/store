using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Entities.Extensions {

    public static class ModQueryableExtensions {

        public static Task<Mod> FindModByDepString(this IQueryable<Mod> mods,
                                                   string depString) {
            var segments = depString.Split('/');
            var orgSlug = segments[0];
            var modSlug = segments[1];

            return mods
                   .Where(m => m.Slug == modSlug)
                   .SingleAsync(m => m.Owner.Slug == orgSlug);
        }

    }

}