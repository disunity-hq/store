using System;
using Disunity.Store.Shared.Util;

namespace Disunity.Store.Shared.Data.Hooks {

    public class OnBeforeCreate : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called before a new model of any type in <see cref="entityTypes"/> is created 
        /// </summary>
        /// <remarks>
        /// Several values are provided via named attributes to the method:
        /// <list type="table">
        /// <listheader>
        /// <term>Name</term>
        /// <term>Type</term>
        /// <term>Description</term>
        /// </listheader>
        /// <item>
        /// <term>entity</term>
        /// <term>object</term>
        /// <term>The entity instance being operated on</term>
        /// </item>
        /// <item>
        /// <term>entityEntry</term>
        /// <term><see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry"/></term>
        /// <term>The current <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry"/></term>
        /// </item>
        /// <item>
        /// <term>services/serviceProvider</term>
        /// <term><see cref="IServiceProvider"/></term>
        /// <term>The main <see cref="IServiceProvider"/></term>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnBeforeCreate(params Type[] entityTypes) : base(entityTypes) { }

    }

}