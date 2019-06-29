using System;

namespace Disunity.Store.Shared.Data.Hooks {

    public class OnAfterUpdate : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called after an existing model of any type in <see cref="entityTypes"/> is updated
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreate"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnAfterUpdate(params Type[] entityTypes) : base(entityTypes) { }

    }

}