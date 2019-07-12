using Microsoft.Extensions.DependencyInjection;

using BindingAttributes;


namespace Disunity.Store.Shared.Startup.Binders {

    public static class AttributeBinderExt {

        public static void ConfigureAttributes(this IServiceCollection services) {
            BindingAttribute.ConfigureBindings(services);
            FactoryAttribute.ConfigureFactories(services);
        }

    }

}