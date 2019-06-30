using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Disunity.Store.Shared.Startup;
using Disunity.Store.Shared.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Data.Hooks {

    [Binding(BindType.Singleton)]
    public class DbHookManager<T> : IDbHookManager<T> where T : DbHookAttribute {

        private readonly IDictionary<DbContext, IDictionary<Type, IList<MethodBase>>> _contextHooks =
            new Dictionary<DbContext, IDictionary<Type, IList<MethodBase>>>();

        private readonly ILogger<DbHookManager<T>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DbHookManager(ILogger<DbHookManager<T>> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void InitializeForContext(DbContext context) {
            var hooks = new Dictionary<Type, IList<MethodBase>>();
            if (!_contextHooks.TryAdd(context, hooks)) {
                throw new InvalidOperationException("Hook Manager has already been initialized with given context");
            }

            var contextPropsTypes = context.GetType()
                                           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                           .Select(p => p.PropertyType)
                                           .Where(t => t.IsGenericType)
                                           .Where(t => typeof(DbSet<>).IsAssignableFrom(t.GetGenericTypeDefinition()));
            foreach (var prop in contextPropsTypes) {
                var classType = prop.GetGenericArguments()[0];
                for (var derivedType = classType;
                     derivedType != typeof(object) && derivedType != null;
                     derivedType = derivedType.BaseType) {
                    foreach (var method in derivedType.GetMethods()) {
                        if (!method.IsStatic) {
                            continue;
                        }

                        var methodAttrs = method.GetCustomAttributes(typeof(T));

                        foreach (T hookAttr in methodAttrs) {
                            var targetEntityTypes = hookAttr.EntityTypes;
                            // Default to containing class if no types specified
                            if (targetEntityTypes.Length == 0) {
                                targetEntityTypes = new[] {classType};
                            }

                            foreach (var entityType in targetEntityTypes) {
                                if (!hooks.ContainsKey(entityType)) {
                                    hooks.Add(entityType, new List<MethodBase>());
                                }

                                hooks[entityType].Add(method);
                                _logger.LogDebug(
                                    $"Registered {method.Name} to listen for {typeof(T).Name} on {entityType.Name}");
                            }
                        }
                    }
                }
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