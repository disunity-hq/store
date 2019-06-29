using System;
using System.Collections.Generic;
using System.Reflection;
using Disunity.Store.Shared.Startup;
using Disunity.Store.Shared.Util;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Data.Hooks {

    [Binding(BindType.Singleton)]
    public class DbHookManager<T> : IDbHookManager<T> where T : DbHookAttribute {

        private readonly IDictionary<Type, IList<MethodBase>> _hooks = new Dictionary<Type, IList<MethodBase>>();
        private readonly ILogger<DbHookManager<T>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DbHookManager(ILogger<DbHookManager<T>> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var classType in assembly.GetTypes()) {
                    for (var derivedType = classType;
                         derivedType != typeof(object) && derivedType != null;
                         derivedType = derivedType.BaseType) {
                        foreach (var method in derivedType.GetMethods()) {
                            if (!method.IsStatic) {
                                continue;
                            }

                            var methodAttrs = method.GetCustomAttributes(typeof(T));

                            foreach (T hookAttr in methodAttrs) {

                                var entityTypes = hookAttr.EntityTypes;
                                // Default to containing class if no types specified
                                if (entityTypes.Length == 0) {
                                    entityTypes = new[] {classType};
                                }

                                foreach (var entityType in entityTypes) {
                                    if (!_hooks.ContainsKey(entityType)) {
                                        _hooks.Add(entityType, new List<MethodBase>());
                                    }

                                    _hooks[entityType].Add(method);
                                    logger.LogDebug(
                                        $"Registered {method.Name} to listen for {typeof(T).Name} on {entityType.Name}");
                                }
                            }
                        }
                    }
                }
            }

            logger.LogDebug($"Registered {typeof(T).Name} for {_hooks.Keys.Count} types");
        }

        public void ExecuteForEntity(EntityEntry entityEntry) {
            var entityName = entityEntry.Entity.GetType().Name;
            _logger.LogInformation(
                $"Entity Type: {entityName}. Hook Type: {typeof(T).Name} Total Hook Types: {_hooks.Keys.Count}");
            var entityType = entityEntry.Entity.GetType();
            if (!_hooks.ContainsKey(entityType)) {
                _logger.LogDebug($"No hooks for {entityName} {typeof(T).Name}");
                return;
            }

            var methods = _hooks[entityType];

            var namedArgs = new Dictionary<string, object>() {
                {"entity", entityEntry.Entity},
                {"entityEntry", entityEntry},
                {"services", _serviceProvider},
                {"serviceProvider", _serviceProvider},
            };

            foreach (var method in methods) {
                _logger.LogInformation($"Executing method {method.Name}");
                method.InvokeWithNamedParameters(null, namedArgs);
            }
        }

    }

}