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
        protected Object _serviceType;

        public Binding(BindType bindType, Type serviceType) {
            _bindType = bindType;
            _serviceType = serviceType;
        }

        public Binding(BindType bindType) {
            _bindType = bindType;
        }

        public Binding(Type serviceType) {
            _bindType = BindType.Transient;
            _serviceType = serviceType;
        }

        public Binding() {
            _bindType = BindType.Transient;
        }

        public void Bind(IServiceCollection services, Type serviceType, Type implementationType) {
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(serviceType, implementationType);
                    break;

                case BindType.Scoped:
                    services.AddScoped(serviceType, implementationType);
                    break;

                case BindType.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
            }
        }

        public void BindWith(IServiceCollection services, Type serviceType, MethodInfo handler) {
            var closure = new Func<IServiceProvider, object>(s => handler.Invoke(null, new[] {s}));
            switch (_bindType) {
                case BindType.Singleton:
                    services.AddSingleton(serviceType, closure);
                    break;

                case BindType.Scoped:
                    services.AddScoped(serviceType, closure);
                    break;

                case BindType.Transient:
                    services.AddTransient(serviceType, closure);
                    break;
            }
        }

        public void Execute(IServiceCollection services, Type implementationType) {
            if (_serviceType == null) {
                Bind(services, implementationType, implementationType);
            } else {
                Bind(services, (Type) _serviceType, implementationType);
            }
        }

        public void ExecuteWith(IServiceCollection services, Type implementationType, MethodInfo handler) {
            if (_serviceType == null) {
                BindWith(services, implementationType, handler);
            } else {
                BindWith(services, (Type) _serviceType, handler);
            }
        }

        public static void ConfigureBindings(IServiceCollection services) {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var implementationType in assembly.GetTypes()) {
                    var classAttrs = implementationType.GetCustomAttributes(typeof(Binding));

                    foreach (Binding attr in classAttrs) {
                        attr.Execute(services, implementationType);
                    }

                    foreach (var method in implementationType.GetRuntimeMethods()) {
                        if (!method.IsStatic) {
                            continue;
                        }

                        var methodAttrs = method.GetCustomAttributes(typeof(Binding));

                        if (methodAttrs.Count() == 0) {
                            continue;
                        }

                        foreach (Binding attr in methodAttrs) {
                            attr.ExecuteWith(services, implementationType, method);
                        }
                    }
                }
            }
        }

    }

}