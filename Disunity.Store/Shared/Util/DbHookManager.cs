using System;
using System.Collections.Generic;
using System.Reflection;
using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Data.Hooks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Util
{
    public static class DbHookManager<T> where T : DbHookAttribute
    {
        private static readonly IDictionary<Type, IList<MethodBase>> Hooks = new Dictionary<Type, IList<MethodBase>>();


        public static void LoadAllHooks(ILogger<ApplicationDbContext> logger)
        {
            Hooks.Clear();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var classType in assembly.GetTypes())
                {
                    foreach (var method in classType.GetMethods())
                    {
                        if (!method.IsStatic)
                        {
                            continue;
                        }

                        var methodAttrs = method.GetCustomAttributes(typeof(T));

                        foreach (T hookAttr in methodAttrs)
                        {
                            logger.LogInformation($"Found hook {method.Name} for {typeof(T).Name}");

                            var entityTypes = hookAttr.EntityTypes;
                            // Default to containing class if no types specified
                            if (entityTypes.Length == 0)
                            {
                                entityTypes = new[] {classType};
                            }

                            foreach (var entityType in entityTypes)
                            {
                                if (!Hooks.ContainsKey(entityType))
                                {
                                    Hooks.Add(entityType, new List<MethodBase>());
                                }

                                Hooks[entityType].Add(method);
                                logger.LogInformation(
                                    $"Registered {method.Name} to listen for {typeof(T).Name} on {entityType.Name}");
                            }
                        }
                    }
                }
            }
            
            logger.LogInformation($"Registers hooks for {Hooks.Keys.Count} types");
        }

        public static void ExecuteForEntity(EntityEntry entityEntry, IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            var entityName = entityEntry.Entity.GetType().Name;
            logger.LogInformation($"Entity Type: {entityName}. Hook Type: {typeof(T).Name} Total Hook Types: {Hooks.Keys.Count}");
            var entityType = entityEntry.Entity.GetType();
            if (!Hooks.ContainsKey(entityType))
            {
                logger.LogWarning($"No hooks for {entityName} {typeof(T).Name}");
                return;
            }

            var methods = Hooks[entityType];

            var namedArgs = new Dictionary<string, object>()
            {
                {"entity", entityEntry.Entity},
                {"entityEntry", entityEntry},
                {"services", serviceProvider},
                {"serviceProvider", serviceProvider},
            };

            foreach (var method in methods)
            {
                logger.LogInformation($"Executing method {method.Name}");
                method.InvokeWithNamedParameters(null, namedArgs);
            }
        }
    }
}