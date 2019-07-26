using System;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Extensions;
using Disunity.Store.Policies;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Route("api/v{version:apiVersion}/mods/{orgSlug:slug}/{modSlug:slug}/{versionNumber:semver}")]
    public class ModPublishingController : ControllerBase {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ModPublishingController> _logger;

        [FromRoute] public string OrgSlug { get; set; }
        [FromRoute] public string ModSlug { get; set; }
        [FromRoute] public string VersionNumber { get; set; }

        public ModPublishingController(ApplicationDbContext dbContext, ILogger<ModPublishingController> logger) {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("publish")]
        [ModOperation(Operation.Update)]
        public async Task<ActionResult> PublishMod() {
            var modVersion = await GetModVersionAsync();

            if (modVersion == null || modVersion.IsActive != false) {
                return NotFound();
            }

            modVersion.IsActive = true;
            await _dbContext.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete]
        [ModOperation(Operation.Delete)]
        public async Task<ActionResult> DeleteMod() {
            var modVersion = await GetModVersionAsync();

            if (modVersion == null) {
                return NotFound();
            }

            if (modVersion.IsActive == true) {
                return Forbid();
            }

            var versionEntry = _dbContext.Entry(modVersion);
            versionEntry.State = EntityState.Deleted;

            if (modVersion.Mod.Latest == modVersion) {
                var modEntry = _dbContext.Entry(modVersion.Mod);
                modEntry.State = EntityState.Deleted;
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        private async Task<ModVersion> GetModVersionAsync() {
            return await _dbContext.ModVersions
                                   .Include(v => v.Mod)
                                   .Where(v => v.Mod.Owner.Slug == OrgSlug && v.Mod.Slug == ModSlug)
                                   .FindExactVersion(VersionNumber)
                                   .SingleOrDefaultAsync();
        }

    }

}