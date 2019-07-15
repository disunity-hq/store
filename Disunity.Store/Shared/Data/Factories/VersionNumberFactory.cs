using System;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Shared.Data.Factories {

    [AsScoped(typeof(IVersionNumberFactory))]
    public class VersionNumberFactory : IVersionNumberFactory {

        private readonly ApplicationDbContext _context;

        public VersionNumberFactory(ApplicationDbContext context) {
            _context = context;

        }

        public Task<VersionNumber> FindOrCreateVersionNumber(string versionString) {
            return FindOrCreateVersionNumber(new VersionNumber(versionString));
        }

        public async Task<VersionNumber> FindOrCreateVersionNumber(VersionNumber versionNumber) {
            var existingVersionNumber = await _context.VersionNumbers.SingleOrDefaultAsync(
                v => v.Major == versionNumber.Major &&
                     v.Minor == versionNumber.Minor &&
                     v.Patch == versionNumber.Patch);

            if (existingVersionNumber != null) {
                return existingVersionNumber;
            }

            _context.VersionNumbers.Attach(versionNumber);
            await _context.SaveChangesAsync();

            return versionNumber;
        }

    }

}