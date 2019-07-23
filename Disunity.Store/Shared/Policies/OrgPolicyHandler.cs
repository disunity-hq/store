using System;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Policies.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Policies {

    [AsScoped(typeof(IAuthorizationHandler))]
    public class OrgPolicyHandler : IAuthorizationHandler {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILogger<OrgPolicyHandler> _logger;

        public OrgPolicyHandler(ApplicationDbContext dbContext,
                                IActionContextAccessor actionContextAccessor,
                                UserManager<UserIdentity> userManager,
                                ILogger<OrgPolicyHandler> logger) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _actionContextAccessor = actionContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        private async Task<bool> IsAdmin(UserIdentity user) {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains(UserRoles.Admin.ToString());
        }

        private string GetOrgSlug() {
            var routeValues = _actionContextAccessor.ActionContext.RouteData.Values;

            if (routeValues.ContainsKey("orgSlug")) {
                return (string) _actionContextAccessor.ActionContext.RouteData.Values["orgSlug"];
            }

            if (routeValues.ContainsKey("userSlug")) {
                return (string) _actionContextAccessor.ActionContext.RouteData.Values["userSlug"];
            }

            return null;
        }

        private async Task<UserIdentity> GetUser() {
            var httpContext = _actionContextAccessor.ActionContext.HttpContext;
            return await _userManager.GetUserAsync(httpContext.User);
        }

        private async Task<Org> GetOrg(string slug) {
            return await _dbContext.Orgs.SingleAsync(o => o.Slug == slug);
        }

        private async Task<(Org, UserIdentity)> GetOrgAndUser() {
            var user = await GetUser();
            var slug = GetOrgSlug();
            var org = await GetOrg(slug);
            return (org, user);
        }

        private async Task<bool> HasRole(OrgMemberRole? role) {
            var (org, user) = await GetOrgAndUser();

            if (org == null || user == null) {
                return false;
            }

            OrgMember membership;

            if (role == null) {
                membership = _dbContext.OrgMembers.Single(o => o.Org == org && o.User == user);
            } else {
                membership = _dbContext.OrgMembers.Single(o => o.Org == org && o.User == user && o.Role == role);
            }

            return membership != null;
        }

        private async Task<bool> IsOrgMember() {
            return await HasRole(OrgMemberRole.Member);
        }

        private async Task<bool> IsOrgAdmin() {
            return await HasRole(OrgMemberRole.Admin);
        }

        private async Task<bool> IsOrgOwner() {
            return await HasRole(OrgMemberRole.Owner);
        }

        private async Task<bool> CanManageOrg() {
            var isOrgAdmin = await IsOrgAdmin();
            var isOrgOwner = await IsOrgOwner();
            return isOrgAdmin || isOrgOwner;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context) {
            var isAdmin = context.User.IsInRole(UserRoles.Admin.ToString());

            foreach (var requirement in context.PendingRequirements.ToList()) {
                _logger.LogWarning($"Checking requirement: {requirement.GetType().Name}");

                var authorized =
                    requirement is CanManageOrg && (isAdmin || await CanManageOrg()) ||
                    requirement is HasOrgRole req && await HasRole(req.Role);

                if (authorized) {
                    _logger.LogWarning($"Authorized!");
                    context.Succeed(requirement);
                }
            }

        }

    }

}