using System.Linq;
using System.Reflection;
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

        public void ProcessMethodAttribute(ILogger<OperationFilter> logger,
                                           AuthorizationFilterContext context,
                                           IAuthorizationService authService,
                                           OperationAttribute attr) {
            var resource = attr.GetResource(context);

            if (resource == null) {
                logger.LogDebug("Resource was null.");
                context.Result = new ForbidResult();
                return;
            }

            var authorization = authService.AuthorizeAsync(context.HttpContext.User, resource, attr.Operation).Result;

            logger.LogDebug($"Authorization succeeded: {authorization.Succeeded}");

            if (!authorization.Succeeded) {
                context.Result = new ForbidResult();
            }
        }

        public void ProcessMethodAttributes(ILogger<OperationFilter> logger,
                                            AuthorizationFilterContext context,
                                            IAuthorizationService authService,
                                            MethodInfo methodInfo) {
            var attrs = methodInfo.GetCustomAttributes(typeof(OperationAttribute), true);

            logger.LogDebug($"Found {attrs.Length} OperationAttributes.");

            foreach (OperationAttribute attr in attrs) {
                ProcessMethodAttribute(logger, context, authService, attr);
            }

        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OperationFilter>>();
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var desc = context.ActionDescriptor as ControllerActionDescriptor;

            if (desc == null) {
                logger.LogDebug($"ControllerActionDescriptor was null: {context.ActionDescriptor.DisplayName}");
                return;
            }

            ProcessMethodAttributes(logger, context, authService, desc.MethodInfo);
        }

    }

}