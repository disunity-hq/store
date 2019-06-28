using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Binder = System.Action<Microsoft.Extensions.DependencyInjection.IServiceCollection>;

namespace Disunity.Store.Shared.Startup {

    public enum BindType {

        Singleton,
        Transient,
        Scoped

    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class Binding : Attribute {

        protected BindType _bindType;
        protected Object _targetType;

        public Binding(BindType bindType, Type targetType) {
            _bindType = bindType;
            _targetType = targetType;
        }

        public Binding(BindType bindType) {
            _bindType = bindType;
        }

        public void BindInterface(IServiceCollection services, Type sourceType) {
            Type targetType = (Type) _targetType;
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(targetType, sourceType);
                    break;
                case BindType.Scoped:
                    services.AddScoped(targetType, sourceType);
                    break;
                case BindType.Transient:
                    services.AddTransient(targetType, sourceType);
                    break;
            }
        }

        public void Bind(IServiceCollection services, Type sourceType) {
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(sourceType);
                    break;
                case BindType.Scoped:
                    services.AddScoped(sourceType);
                    break;
                case BindType.Transient:
                    services.AddTransient(sourceType);
                    break;
            }
        }

        public void BindInterfaceWith(IServiceCollection services, Type sourceType, MethodInfo handler) {
            Type targetType = (Type) _targetType;
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(targetType, s => handler.Invoke(null, new[] { s as object }));
                    break;
                case BindType.Scoped:
                    services.AddScoped(targetType, s => handler.Invoke(null, new[] { s as object }));
                    break;
                case BindType.Transient:
                    services.AddTransient(targetType, s => handler.Invoke(null, new[] { s as object }));
                    break;
            }
        }

        public void BindWith(IServiceCollection services, Type sourceType, MethodInfo handler) {
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(sourceType, (s) => handler.Invoke(null, new[] {s as object}));
                    break;
                case BindType.Scoped:
                    services.AddScoped(sourceType, (s) => handler.Invoke(null, new[] {s as object}));
                    break;
                case BindType.Transient:
                    services.AddTransient(sourceType, (s) => handler.Invoke(null, new[] {s as object}));
                    break;
            }
        }

        public void Execute(IServiceCollection services, Type sourceType) {
            if (_targetType == null) {
                Bind(services, sourceType);
            } else {
                BindInterface(services, sourceType);
            } 
        }

        public void ExecuteWith(IServiceCollection services, Type sourceType, MethodInfo handler) {
            if (_targetType == null) {
                BindWith(services, sourceType, handler);
            } else {
                BindInterfaceWith(services, sourceType, handler);
            }
        }

        public static void ConfigureBindings(IServiceCollection services) {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    var classAttrs = type.GetCustomAttributes(typeof(Binding));

                    foreach (Binding attr in classAttrs) {
                        attr.Execute(services, type);
                    }

                    foreach (var method in type.GetRuntimeMethods()) {
                        if (!method.IsStatic) {
                            continue;
                        }

                        var methodAttrs = method.GetCustomAttributes(typeof(Binding));

                        if (methodAttrs.Count() == 0) {
                            continue;
                        }
                        
                        foreach (Binding attr in methodAttrs) {
                            attr.ExecuteWith(services, type, method);
                        }
                    }
                }
            }
            
        }

    }

}