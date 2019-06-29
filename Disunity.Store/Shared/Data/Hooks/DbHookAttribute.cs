using System;
using System.Collections.Generic;
using System.Reflection;
using Disunity.Store.Shared.Util;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Data.Hooks
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class DbHookAttribute : Attribute
    {
        public Type[] EntityTypes { get; }


        protected DbHookAttribute(params Type[] entityTypes)
        {
            EntityTypes = entityTypes;
        }
    }
}