using System;

namespace Disunity.Store.Shared.Data
{
    public interface IBeforeCreate
    {
        /// <summary>
        /// Called before model is added to database
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        void OnBeforeCreate(IServiceProvider serviceProvider);
    }

    public interface IBeforeUpdate
    {
        /// <summary>
        /// Called before a model already in the database is modified
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        void OnBeforeUpdate(IServiceProvider serviceProvider);
    }

    public interface IBeforeSave
    {
        /// <summary>
        /// Called before a model is created or updated.
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        /// <see cref="IBeforeCreate"/>
        /// <see cref="IBeforeUpdate"/>
        void OnBeforeSave(IServiceProvider serviceProvider);
    }

    public interface IBeforeDelete
    {
        /// <summary>
        /// Called before a model in the database is deleted
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        void OnBeforeDelete(IServiceProvider serviceProvider);
    }

    public interface IAfterCreate
    {
        /// <summary>
        /// Called after model is added to database
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        void OnAfterCreate(IServiceProvider serviceProvider);
    }

    public interface IAfterUpdate
    {
        /// <summary>
        /// Called after a model already in the database is modified
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        void OnAfterUpdate(IServiceProvider serviceProvider);
    }

    public interface IAfterSave
    {
        /// <summary>
        /// Called before a model is created or updated.
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        /// <see cref="IAfterCreate"/>
        /// <see cref="IAfterUpdate"/>
        void OnAfterSave(IServiceProvider serviceProvider);
    }

    public interface IAfterDelete
    {
        /// <summary>
        /// Called before a model in the database is deleted
        /// </summary>
        /// <param name="serviceProvider">Service provider for accessing necessary dependencies during the hook</param>
        void OnAfterDelete(IServiceProvider serviceProvider);
    }
   
    
}