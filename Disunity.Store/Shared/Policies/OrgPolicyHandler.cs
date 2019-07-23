using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Policies.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Policies {

    [AsScoped(typeof(IAuthorizationHandler))]
    public class OrgPolicyHandler : AuthorizationHandler<OperationAuthorizationRequirement, Org> {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrgPolicyHandler> _logger;

        public OrgPolicyHandler(ApplicationDbContext dbContext, ILogger<OrgPolicyHandler> logger) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        private void ReadOp(AuthorizationHandlerContext context,
                            OperationAuthorizationRequirement requirement,
                            OrgMember membership) {
            context.Succeed(requirement);
        }

        private void CreateOp(AuthorizationHandlerContext context,
                              OperationAuthorizationRequirement requirement,
                              OrgMember membership) {
            context.Succeed(requirement);
        }

        private void UpdateOp(AuthorizationHandlerContext context,
                              OperationAuthorizationRequirement requirement,
                              OrgMember membership) {
            if (membership != null) {
                context.Succeed(requirement);
            }
        }

        private void DeleteOp(AuthorizationHandlerContext context,
                              OperationAuthorizationRequirement requirement,
                              OrgMember membership) {
            if (membership != null && membership.Role == OrgMemberRole.Owner)
                context.Succeed(requirement);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Org resource) {
            if (context.User.IsInRole(UserRoles.Admin.ToString())) {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var membership = _dbContext.OrgMembers.SingleOrDefault(m => m.Org == resource && m.User.Id == userId);

            var handlers =
                new Dictionary<string,
                    Action<AuthorizationHandlerContext,
                        OperationAuthorizationRequirement,
                        OrgMember>> {
                    {"Read", ReadOp},
                    {"Create", CreateOp},
                    {"Update", UpdateOp},
                    {"Delete", DeleteOp}
                };

            if (handlers.ContainsKey(requirement.Name)) {
                var handler = handlers[requirement.Name];
                handler(context, requirement, membership);
            }

            return Task.CompletedTask;
        }

    }

}