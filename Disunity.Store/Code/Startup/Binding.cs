using System;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Code.Startup {

    public enum BindType {

        Singleton,
        Transient,
        Scoped

    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class Binding : Attribute {

        protected BindType _bindType;
        protected Type _serviceType;

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
                Bind(services, _serviceType, implementationType);
            }
        }

        public void ExecuteWith(IServiceCollection services, Type implementationType, MethodInfo handler) {
            if (_serviceType == null) {
                BindWith(services, implementationType, handler);
            } else {
                BindWith(services, _serviceType, handler);
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

    public class AsSingleton : Binding {

        public AsSingleton() : base(BindType.Singleton) { }

        public AsSingleton(Type serviceType) : base(BindType.Singleton, serviceType) { }

    }

    public class AsTransient : Binding {

        public AsTransient() : base(BindType.Transient) { }

        public AsTransient(Type serviceType) : base(BindType.Transient, serviceType) { }

    }

    public class AsScoped : Binding {

        public AsScoped() : base(BindType.Scoped) { }

        public AsScoped(Type serviceType) : base(BindType.Scoped, serviceType) { }

    }

}