using Disunity.Store.Shared.Startup;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Shared.Data.Hooks {

    [Binding(BindType.Singleton)]
    public class HookManagerContainer {

        public DbHookManager<OnBeforeCreate> OnBeforeCreate { get; }
        public DbHookManager<OnBeforeUpdate> OnBeforeUpdate { get; }
        public DbHookManager<OnBeforeSave> OnBeforeSave { get; }
        public DbHookManager<OnBeforeDelete> OnBeforeDelete { get; }
        public DbHookManager<OnAfterCreate> OnAfterCreate { get; }
        public DbHookManager<OnAfterUpdate> OnAfterUpdate { get; }
        public DbHookManager<OnAfterSave> OnAfterSave { get; }
        public DbHookManager<OnAfterDelete> OnAfterDelete { get; }


        public HookManagerContainer(DbHookManager<OnBeforeCreate> onBeforeCreate,
                                    DbHookManager<OnBeforeUpdate> onBeforeUpdate,
                                    DbHookManager<OnBeforeSave> onBeforeSave,
                                    DbHookManager<OnBeforeDelete> onBeforeDelete,
                                    DbHookManager<OnAfterCreate> onAfterCreate,
                                    DbHookManager<OnAfterUpdate> onAfterUpdate,
                                    DbHookManager<OnAfterSave> onAfterSave,
                                    DbHookManager<OnAfterDelete> onAfterDelete) {
            OnBeforeCreate = onBeforeCreate;
            OnBeforeUpdate = onBeforeUpdate;
            OnBeforeSave = onBeforeSave;
            OnBeforeDelete = onBeforeDelete;
            OnAfterCreate = onAfterCreate;
            OnAfterUpdate = onAfterUpdate;
            OnAfterSave = onAfterSave;
            OnAfterDelete = onAfterDelete;
        }

        public void InitializeForAll(DbContext context) {
            OnBeforeCreate.InitializeForContext(context);
            OnBeforeUpdate.InitializeForContext(context);
            OnBeforeSave.InitializeForContext(context);
            OnBeforeDelete.InitializeForContext(context);
            OnAfterCreate.InitializeForContext(context);
            OnAfterUpdate.InitializeForContext(context);
            OnAfterSave.InitializeForContext(context);
            OnAfterDelete.InitializeForContext(context);
        }

    }

}