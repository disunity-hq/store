using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Shared.Extensions {

    public static class DbContextExtensions {

        public static IEnumerable<Type> DbSetTypes(this DbContext context) {
            return context.GetType()
                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          .Select(p => p.PropertyType)
                          .Where(t => t.IsGenericType)
                          .Where(t => typeof(DbSet<>).IsAssignableFrom(t.GetGenericTypeDefinition()));
        }    

    }

}