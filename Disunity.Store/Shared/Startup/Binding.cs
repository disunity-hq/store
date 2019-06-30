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

        public Binding(Type targetType) {
            _bindType = BindType.Transient;
            _targetType = targetType;
        }

        public Binding() {
            _bindType = BindType.Transient;
        }

        public void Bind(IServiceCollection services, Type type) {
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(type);
                    break;

                case BindType.Scoped:
                    services.AddScoped(type);
                    break;

                case BindType.Transient:
                    services.AddTransient(type);
                    break;
            }
        }

        public void BindWith(IServiceCollection services, Type type, MethodInfo handler) {
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(type, (s) => handler.Invoke(null, new[] {s as object}));
                    break;

                case BindType.Scoped:
                    services.AddScoped(type, (s) => handler.Invoke(null, new[] {s as object}));
                    break;

                case BindType.Transient:
                    services.AddTransient(type, (s) => handler.Invoke(null, new[] {s as object}));
                    break;
            }
        }

        public void Execute(IServiceCollection services, Type sourceType) {
            if (_targetType == null) {
                Bind(services, sourceType);
            } else {
                Bind(services, (Type) _targetType);
            }
        }

        public void ExecuteWith(IServiceCollection services, Type sourceType, MethodInfo handler) {
            if (_targetType == null) {
                BindWith(services, sourceType, handler);
            } else {
                BindWith(services, (Type) _targetType, handler);
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