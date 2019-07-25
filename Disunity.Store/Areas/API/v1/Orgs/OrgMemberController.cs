using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Policies;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Areas.API.v1.Orgs {

    [ApiController]
    [Route("api/v{version:apiVersion}/orgs/{orgSlug:slug}/members")]
    public class OrgMemberController : ControllerBase {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrgMemberController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// The human-readable unique identifier for this org
        /// </summary>
        [FromRoute(Name = "orgSlug")]
        public string OrgSlug { get; set; }

        public OrgMemberController(ApplicationDbContext dbContext, ILogger<OrgMemberController> logger,
                                   IMapper mapper) {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

        }

        /// <summary>
        /// Get a list of the memberships for all users in the given org
        /// </summary>
        /// <returns>A JSON array of all the members and their roles within this org</returns>
        [HttpGet]
        [OrgOperation("orgSlug", Operation.Read)]
        public async Task<ActionResult<IEnumerable<OrgMemberDto>>> GetMembersAsync() {
            var memberships = await _dbContext.OrgMembers
                                              .Include(m => m.User)
                                              .Where(m => m.Org.Slug == OrgSlug)
                                              .ProjectTo<OrgMemberDto>(_mapper.ConfigurationProvider)
                                              .ToListAsync();

            return new JsonResult(memberships);
        }

        /// <summary>
        /// Adds a user to an existing organization
        /// </summary>
        /// <param name="membershipDto">The username of the user to add and the role with which to grant them</param>
        /// <returns>Status code 204 for success or 400 for a bad request</returns>
        /// <response code="204">Indicates that the user was added successfully</response>
        /// <response code="400">Returns information about why the request was malformed</response>
        [HttpPost]
        [OrgOperation("orgSlug", Operation.ManageMembers)]
        public async Task<IActionResult> AddOrgMember([FromBody] OrgMemberDto membershipDto) {
            if (membershipDto.Role == OrgMemberRole.Owner) {
                ModelState.AddModelError("invalidRole", "Cannot add extra owner's to an organization");
                return GetResultFromModelState();
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == membershipDto.UserName);

            if (user == null) {
                ModelState.AddModelError("noSuchUser", $"No user exists with username {membershipDto.UserName}");
                return GetResultFromModelState();
            }

            var org = await _dbContext.Orgs.SingleAsync(o => o.Slug == OrgSlug);

            var membership = new OrgMember() {
                User = user,
                Org = org,
                Role = membershipDto.Role
            };

            _dbContext.Add(membership);
            await _dbContext.SaveChangesAsync();


            return GetResultFromModelState();
        }

        /// <summary>
        /// Remove a user from an organization
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username:slug}")]
        [OrgOperation("orgSlug", Operation.ManageMembers)]
        public async Task<IActionResult> RemoveOrgMember([FromRoute] string username) {
            var membership =
                await _dbContext.OrgMembers.SingleOrDefaultAsync(
                    m => m.Org.Slug == OrgSlug && m.User.UserName == username);

            if (membership == null) {
                return NotFound();
            }

            _dbContext.OrgMembers.Remove(membership);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Update a users role within an org
        /// </summary>
        /// <param name="membershipDto"></param>
        /// <returns></returns>
        [HttpPut]
        [OrgOperation("orgSlug", Operation.ManageMemberRoles)]
        public async Task<IActionResult> UpdateMemberRole([FromBody] OrgMemberDto membershipDto) {
            var membership =
                await _dbContext.OrgMembers.SingleOrDefaultAsync(
                    m => m.Org.Slug == OrgSlug && m.User.UserName == membershipDto.UserName);

            if (membership == null) {
                return NotFound();
            }

            membership.Role = membershipDto.Role;
            
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private IActionResult GetResultFromModelState() {
            if (ModelState.ErrorCount > 0) {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }

}