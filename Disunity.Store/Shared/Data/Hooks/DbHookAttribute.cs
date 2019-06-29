using System;

namespace Disunity.Store.Shared.Data.Hooks {

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class DbHookAttribute : Attribute {

        public Type[] EntityTypes { get; }


        protected DbHookAttribute(params Type[] entityTypes) {
            EntityTypes = entityTypes;
        }

    }

}