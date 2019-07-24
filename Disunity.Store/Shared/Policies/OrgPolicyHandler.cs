using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class OrgPolicyHandler : AuthorizationHandler<OperationRequirement, Org> {

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrgPolicyHandler> _logger;

        public OrgPolicyHandler(ApplicationDbContext dbContext, ILogger<OrgPolicyHandler> logger) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        private static bool ReadOp(OrgMember membership) {
            return true;
        }

        private static bool CreateOp(OrgMember membership) {
            return true;
        }

        private static bool UpdateOp(OrgMember membership) {
            return membership != null;
        }

        private static bool DeleteOp(OrgMember membership) {
            return membership != null && membership.Role == OrgMemberRole.Owner;
        }

        private MethodInfo GetHandler(OperationRequirement requirement) {
            var name = $"{requirement.Operation.ToString()}Op";
            var type = typeof(OrgPolicyHandler);
            return type.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
        } 

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationRequirement requirement,
                                                       Org resource) {
            _logger.LogDebug($"Handling OperationRequirement: {requirement.Operation.ToString()}");
            if (context.User.IsInRole(UserRoles.Admin.ToString())) {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var membership = _dbContext.OrgMembers.SingleOrDefault(m => m.Org == resource && m.User.Id == userId);
            var methodInfo = GetHandler(requirement);

            if (methodInfo != null) {
                var authorized = (bool) methodInfo.Invoke(null, new object[] {membership});

                if (authorized) {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

    }

}