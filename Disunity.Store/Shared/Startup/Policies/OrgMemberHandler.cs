using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Startup.Policies {

    [AsScoped(typeof(IAuthorizationHandler))]
    public class OrgMemberHandler : AuthorizationHandler<OrgMemberRequirement> {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILogger<OrgMemberHandler> _logger;

        public OrgMemberHandler(ApplicationDbContext dbContext, IActionContextAccessor actionContextAccessor,
                                UserManager<UserIdentity> userManager, ILogger<OrgMemberHandler> logger) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _actionContextAccessor = actionContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                             OrgMemberRequirement requirement) {
            if (await IsMember()) {
                context.Succeed(requirement);
            }

        }

        private async Task<bool> IsMember() {
            var httpContext = _actionContextAccessor.ActionContext.HttpContext;
            var routeValues = _actionContextAccessor.ActionContext.RouteData.Values;

            if (!routeValues.ContainsKey("orgSlug")) {
                return false;
            }

            var slug = (string) _actionContextAccessor.ActionContext.RouteData.Values["orgSlug"];

            var user = await _userManager.GetUserAsync(httpContext.User);

            if (user == null) {
                return false;
            }

            var org = _dbContext.Orgs.Single(o => o.Slug == slug);

            if (org == null) {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains(UserRoles.Admin.ToString())) {
                var membership = _dbContext.OrgMembers.Single(o => o.Org == org && o.User == user);

                if (membership == null) {
                    return false;
                }
            }

            return true;
        }

    }

}