using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Shared.Data
{
    public interface IAutoModel<T> where T : DbContext
    {
        /// <summary>
        /// Called before model is added to database
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        void OnBeforeCreate(T dbContext);

        /// <summary>
        /// Called after model is added to database
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        void OnAfterCreate(T dbContext);

        /// <summary>
        /// Called before a model already in the database is modified
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        void OnBeforeUpdate(T dbContext);

        /// <summary>
        /// Called after a model already in the database is modified
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        void OnAfterUpdate(T dbContext);

        /// <summary>
        /// Called before a model in the database is deleted
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        void OnBeforeDelete(T dbContext);

        /// <summary>
        /// Called before a model in the database is deleted
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        void OnAfterDelete(T dbContext);

        /// <summary>
        /// Called before a model is created or updated.
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        /// <see cref="OnBeforeCreate"/>
        /// <see cref="OnBeforeUpdate"/>
        void OnBeforeSave(T dbContext);
        
        /// <summary>
        /// Called before a model is created or updated.
        /// </summary>
        /// <param name="dbContext">The database context entity is attached to</param>
        /// <see cref="OnAfterCreate"/>
        /// <see cref="OnAfterUpdate"/>
        void OnAfterSave(T dbContext);
    }
}
