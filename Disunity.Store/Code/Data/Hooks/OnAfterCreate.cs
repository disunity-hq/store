using System;


namespace Disunity.Store.Code.Data.Hooks {

    public class OnAfterCreate : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called after a new model of any type in <see cref="entityTypes"/> is created 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreate"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnAfterCreate(params Type[] entityTypes) : base(entityTypes) { }

    }

}