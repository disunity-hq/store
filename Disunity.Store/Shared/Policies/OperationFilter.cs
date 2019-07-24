using System.Linq;
using System.Security.Claims;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Policies {

    public class OperationFilter : IAuthorizationFilter {

        public void OnAuthorization(AuthorizationFilterContext context) {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OperationFilter>>();
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            logger.LogWarning($"Descriptor: {context.ActionDescriptor.GetType().Name}");
            var desc = context.ActionDescriptor as ControllerActionDescriptor;

            if (desc == null) {
                logger.LogWarning("descriptor was null");
                return;
            }

            var attrs = desc.MethodInfo.GetCustomAttributes(typeof(OperationAttribute), true);

            logger.LogWarning($"found {attrs.Length} attrs on {desc.MethodInfo.Name}");

            foreach (OperationAttribute attr in attrs) {
                logger.LogWarning($"Found attr: {attr.GetType().Name}");
                logger.LogWarning($"With op: {attr.Operation}");

                var requirement = attr.GetRequirement();

                if (requirement == null) {
                    logger.LogWarning("Requirement was null.");
                    context.Result = new ForbidResult();
                    return;
                }

                var resource = attr.GetResource(context);

                if (resource == null) {
                    logger.LogWarning("Resource was null.");
                    context.Result = new ForbidResult();
                    return;
                }


                var authorization = authService.AuthorizeAsync(context.HttpContext.User, resource, requirement).Result;
                logger.LogWarning($"The authz succeeded: {authorization.Succeeded}");

                if (!authorization.Succeeded) {
                    context.Result = new ForbidResult();
                }
            }
        }

    }

}