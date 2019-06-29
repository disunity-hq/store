using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Disunity.Store.Shared.Data.Hooks {

    public interface IDbHookManager<T> where T:DbHookAttribute {

        void ExecuteForEntity(EntityEntry entityEntry);

    }

}