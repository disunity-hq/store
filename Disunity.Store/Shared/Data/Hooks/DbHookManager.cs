using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Disunity.Store.Shared.Extensions;
using Disunity.Store.Shared.Startup;
using Disunity.Store.Shared.Util;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Shared.Data.Hooks {

    [Binding(BindType.Singleton, typeof(IDbHookManager<>))]
    public class DbHookManager<T> : IDbHookManager<T> where T : DbHookAttribute {

        private class HookMap : Dictionary<Type, IList<MethodBase>> { }

        private readonly IDictionary<DbContext, HookMap> _contextHooks = new Dictionary<DbContext, HookMap>();
        private readonly ILogger<DbHookManager<T>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DbHookManager(ILogger<DbHookManager<T>> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private void RegisterHandler(HookMap hooks, Type entityType, MethodBase method) {
            if (!hooks.ContainsKey(entityType)) {
                hooks.Add(entityType, new List<MethodBase>());
            }

            hooks[entityType].Add(method);

            _logger.LogDebug(
                $"Registered {method.Name} to listen for {typeof(T).Name} on {entityType.Name}");
        }

        private void HandleAttr(HookMap hooks, Type classType, MethodBase method, T hookAttr) {
            var targetEntityTypes = hookAttr.EntityTypes.Length > 0
                ? hookAttr.EntityTypes
                : new[] {classType};

            targetEntityTypes.ForEach(entityType => RegisterHandler(hooks, entityType, method));
        }

        private void HandleMethod(HookMap hooks, Type classType, MethodBase method) {
            foreach (var attr in method.GetCustomAttributes(typeof(T))) {
                HandleAttr(hooks, classType, method, attr as T);
            }

        }

        private void HandleEntityType(HookMap hooks, Type entityType) {
            for (var derivedType = entityType;
                 derivedType != typeof(object) && derivedType != null;
                 derivedType = derivedType.BaseType) {

                var staticMethods = derivedType.GetMethods(BindingFlags.Static | 
                                                           BindingFlags.Public | 
                                                           BindingFlags.NonPublic);

                foreach (var method in staticMethods) {
                    HandleMethod(hooks, entityType, method);
                }
            }
        }

        public void InitializeForContext(DbContext context) {
            var hooks = new HookMap();

            if (!_contextHooks.TryAdd(context, hooks)) {
                throw new InvalidOperationException("Hook Manager has already been initialized with given context");
            }

            foreach (var entityType in context.DbEntityTypes()) {
                HandleEntityType(hooks, entityType);
            }

            _logger.LogDebug($"Registered {typeof(T).Name} for {hooks.Keys.Count} types");
        }

        public void ExecuteForEntity(DbContext context, EntityEntry entityEntry) {
            if (!_contextHooks.ContainsKey(context)) {
                throw new InvalidOperationException(
                    "Must initialize HookManager with context before attempted to execute hooks in that context");
            }

            var hooks = _contextHooks[context];

            var entityName = entityEntry.Entity.GetType().Name;

            _logger.LogInformation(
                $"Entity Type: {entityName}. Hook Type: {typeof(T).Name} Total Hook Types: {hooks.Keys.Count}");

            var entityType = entityEntry.Entity.GetType();

            if (!hooks.ContainsKey(entityType)) {
                _logger.LogDebug($"No hooks for {entityName} {typeof(T).Name}");
                return;
            }

            var methods = hooks[entityType];

            var namedArgs = new Dictionary<string, object>() {
                {"entity", entityEntry.Entity},
                {"entityEntry", entityEntry},
                {"services", _serviceProvider},
                {"serviceProvider", _serviceProvider},
                {"context", context},
                {"dbContext", context}
            };

            foreach (var method in methods) {
                _logger.LogInformation($"Executing method {method.Name}");
                method.InvokeWithNamedParameters(null, namedArgs);
            }
        }

    }

}