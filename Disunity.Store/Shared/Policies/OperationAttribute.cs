using System;
using System.Linq;
using System.Reflection;

using Disunity.Store.Data;

using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Policies {

    public abstract class OperationAttribute : Attribute {

        public readonly Operation Operation;
        public readonly string RouteValue;
        public readonly bool API;

        protected OperationAttribute(string routeValue, Operation operation, bool api = true) {
            RouteValue = routeValue;
            Operation = operation;
            API = api;
        }

        protected abstract object GetResource(AuthorizationFilterContext context, object routeValue);

        private object GetRouteValue(RouteValueDictionary values) {
            return values.ContainsKey(RouteValue) ? values[RouteValue] : null;
        }

        public object GetResource(AuthorizationFilterContext context) {
            var routeValue = GetRouteValue(context.RouteData.Values);
            return GetResource(context, routeValue);
        }

    }

    public class OrgOperationAttribute : OperationAttribute {

        public OrgOperationAttribute(string routeValue, Operation operation) : base(routeValue, operation) { }

        protected override object GetResource(AuthorizationFilterContext context, object routeValue) {
            var orgSlug = (string) routeValue;
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            return dbContext.Orgs.SingleOrDefault(o => o.Slug == orgSlug);
        }

    }

}