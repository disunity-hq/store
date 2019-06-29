using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Disunity.Store.Shared.Data.Hooks {

    public interface IDbHookManager<T> where T:DbHookAttribute {

        void InitializeForContext(DbContext context);
        
        void ExecuteForEntity(DbContext context, EntityEntry entityEntry);
        

    }

}