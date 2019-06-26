namespace Disunity.Store.Shared.Data
{
    public abstract class AutoModel : IAutoModel<ApplicationDbContext>
    {
        public virtual void OnBeforeCreate(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnAfterCreate(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnBeforeUpdate(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnAfterUpdate(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnBeforeDelete(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnAfterDelete(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnBeforeSave(ApplicationDbContext dbContext)
        {
        }

        public virtual void OnAfterSave(ApplicationDbContext dbContext)
        {
        }
    }
}