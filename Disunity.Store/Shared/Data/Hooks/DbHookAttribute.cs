using System;


namespace Disunity.Store.Shared.Data.Hooks {

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class DbHookAttribute : Attribute {

        protected DbHookAttribute(params Type[] entityTypes) {
            EntityTypes = entityTypes;
        }

        public Type[] EntityTypes { get; }

    }

}