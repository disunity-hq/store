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
            var desc = context.ActionDescriptor as ControllerActionDescriptor;

            if (desc == null) {
                return;
            }

            var attrs = desc.MethodInfo.GetCustomAttributes(typeof(OperationAttribute), true);

            foreach (OperationAttribute attr in attrs) {
                var requirement = attr.GetRequirement();

                if (requirement == null) {
                    context.Result = new ForbidResult();
                    return;
                }

                var resource = attr.GetResource(context);

                if (resource == null) {
                    context.Result = new ForbidResult();
                    return;
                }

                var authorization = authService.AuthorizeAsync(context.HttpContext.User, resource, requirement).Result;

                if (!authorization.Succeeded) {
                    context.Result = new ForbidResult();
                }
            }
        }

    }

}