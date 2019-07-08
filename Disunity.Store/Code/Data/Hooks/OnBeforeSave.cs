using System;


namespace Disunity.Store.Code.Data.Hooks {

    public class OnBeforeSave : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called before a new or existing model of any type in <see cref="entityTypes"/> is saved to the database 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreate"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnBeforeSave(params Type[] entityTypes) : base(entityTypes) { }

    }

}