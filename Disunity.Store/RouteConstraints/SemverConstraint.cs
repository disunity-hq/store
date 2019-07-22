using System.Text.RegularExpressions;

using Disunity.Store.Artifacts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Disunity.Store.RouteConstraints {

    public class SemverConstraint:IRouteConstraint {

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
                          RouteDirection routeDirection) {
            var value = values[routeKey]?.ToString();

            return value != null && new Regex(Schema.VERSION_PATTERN).IsMatch(value);
        }

    }

}