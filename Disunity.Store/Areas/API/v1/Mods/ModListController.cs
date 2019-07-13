using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Entities.DataTransferObjects;
using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.API.v1.Mods {

    [ApiController]
    [Route("api/v{version:apiVersion}/mods")]
    public class ModListController : ControllerBase {

        private readonly ApplicationDbContext _context;

        public ModListController(ApplicationDbContext context) {
            _context = context;
        }


        /// <summary>
        /// Get a list of all mods registered with disunity.io
        /// </summary>
        /// <returns>An array of all found mods that are compatible with</returns>
        /// <response code="200">Return a JSON array of all mods registered with disunity.io</response>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ModDto>>> GetAllMods([FromQuery] int page = 0,
                                                                        [FromQuery] int pageSize = 10) {
            var mods = _context.Mods
//                                     .Include(m => m.Versions)
                               .Page(page, pageSize);

            var modList = await mods.ToListAsync();
            return new JsonResult(modList);
        }

        /// <summary>
        /// Get a list of all mods compatible with the given target id
        /// </summary>
        /// <remarks>
        /// Get a paginated list of all mods compatible with the given target id.
        /// If no pagination options are given or if the page is set to 0,
        /// then pagination will be skipped and all matching mods will be returned 
        /// </remarks>
        /// <param name="targetId">The target id to search for compatible mods</param>
        /// <param name="page">The current page of information to display, begins at 1.</param>
        /// <param name="pageSize"></param>
        /// <returns>An array of all found mods that are compatible with</returns>
        /// <response code="200">Return a JSON array of all found mods compatible with the given target id</response>
        [HttpGet("{targetId}")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<ModDto>> GetModsFromTarget([FromRoute] int targetId,
                                                                   [FromQuery] int page = 0,
                                                                   [FromQuery] int pageSize = 10) {
            var targetMods = _context.Mods
                                     .Include(m => m.Versions)
                                     .Select(m => new {
                                         m,
                                         Versions = m.Versions.Where(
                                             v => v.TargetCompatibilities.Any(c => c.TargetId == targetId))
                                     })
                                     .AsEnumerable()
                                     .Select(x => x.m)
                                     .Where(m => m.Versions.Any())
                                     .Page(page, pageSize)
                                     .ToList();

            return new JsonResult(targetMods);
        }
        
    }

}