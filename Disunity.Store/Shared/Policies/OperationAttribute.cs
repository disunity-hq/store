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

        public readonly string Operation;
        private readonly string RouteValue;

        protected OperationAttribute(string routeValue, string operation) {
            RouteValue = routeValue;
            Operation = operation;
        }

        protected abstract object GetResource(AuthorizationFilterContext context, object routeValue);

        private object GetRouteValue(RouteValueDictionary values) {
            return values.ContainsKey(RouteValue) ? values[RouteValue] : null;
        }

        public object GetResource(AuthorizationFilterContext context) {
            var routeValue = GetRouteValue(context.RouteData.Values);
            return GetResource(context, routeValue);
        }

        public OperationAuthorizationRequirement GetRequirement() {
            var type = typeof(Operations);

            var fields = from field in type.GetFields(BindingFlags.Static | BindingFlags.Public)
                         where field.Name == Operation
                         select (OperationAuthorizationRequirement) field.GetValue(null);

            return fields.FirstOrDefault();
        }

    }

    public class OrgOperationAttribute : OperationAttribute {

        public OrgOperationAttribute(string routeValue, string operation) : base(routeValue, operation) { }

        protected override object GetResource(AuthorizationFilterContext context, object routeValue) {
            var orgSlug = (string) routeValue;
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            return dbContext.Orgs.SingleOrDefault(o => o.Slug == orgSlug);
        }

    }

}