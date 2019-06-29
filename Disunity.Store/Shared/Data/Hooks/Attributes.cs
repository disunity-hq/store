using System;
using Disunity.Store.Shared.Util;

namespace Disunity.Store.Shared.Data.Hooks {

    public class OnBeforeCreateAttribute : DbHookAttribute {

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
        public OnBeforeCreateAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

    public class OnBeforeUpdateAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called before an existing model of any type in <see cref="entityTypes"/> is updated
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnBeforeUpdateAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

    public class OnBeforeSaveAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called before a new or existing model of any type in <see cref="entityTypes"/> is saved to the database 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnBeforeSaveAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

    public class OnBeforeDeleteAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called before an existing model of any type in <see cref="entityTypes"/> is deleted 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnBeforeDeleteAttribute(params Type[] entityTypes) : base(entityTypes) { }

        public static void FOO(IServiceProvider serviceProvider) { }

    }

    public class OnAfterCreateAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called after a new model of any type in <see cref="entityTypes"/> is created 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnAfterCreateAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

    public class OnAfterUpdateAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called after an existing model of any type in <see cref="entityTypes"/> is updated
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnAfterUpdateAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

    public class OnAfterSaveAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called after a new or existing model of any type in <see cref="entityTypes"/> is saved to the database 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnAfterSaveAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

    public class OnAfterDeleteAttribute : DbHookAttribute {

        /// <summary>
        /// Register a callback to be called after an existing model of any type in <see cref="entityTypes"/> is deleted 
        /// </summary>
        /// <remarks>
        /// See <see cref="OnBeforeCreateAttribute"/> for more details
        /// </remarks>
        /// <param name="entityTypes">
        /// The entity types that this hook will respond to.
        /// If left empty, the type of the model class will be assumed.
        /// </param>
        public OnAfterDeleteAttribute(params Type[] entityTypes) : base(entityTypes) { }

    }

}