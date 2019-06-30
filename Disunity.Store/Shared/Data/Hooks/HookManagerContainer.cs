using Disunity.Store.Shared.Startup;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Shared.Data.Hooks {

    [Binding(BindType.Singleton)]
    public class HookManagerContainer {

        public IDbHookManager<OnBeforeCreate> OnBeforeCreate { get; }
        public IDbHookManager<OnBeforeUpdate> OnBeforeUpdate { get; }
        public IDbHookManager<OnBeforeSave> OnBeforeSave { get; }
        public IDbHookManager<OnBeforeDelete> OnBeforeDelete { get; }
        public IDbHookManager<OnAfterCreate> OnAfterCreate { get; }
        public IDbHookManager<OnAfterUpdate> OnAfterUpdate { get; }
        public IDbHookManager<OnAfterSave> OnAfterSave { get; }
        public IDbHookManager<OnAfterDelete> OnAfterDelete { get; }


        public HookManagerContainer(IDbHookManager<OnBeforeCreate> onBeforeCreate,
                                    IDbHookManager<OnBeforeUpdate> onBeforeUpdate,
                                    IDbHookManager<OnBeforeSave> onBeforeSave,
                                    IDbHookManager<OnBeforeDelete> onBeforeDelete,
                                    IDbHookManager<OnAfterCreate> onAfterCreate,
                                    IDbHookManager<OnAfterUpdate> onAfterUpdate,
                                    IDbHookManager<OnAfterSave> onAfterSave,
                                    IDbHookManager<OnAfterDelete> onAfterDelete) {
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