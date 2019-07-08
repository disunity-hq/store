using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup.Binders {

    public static class AttributeBinderExt {

        public static void ConfigureAttributes(this IServiceCollection services) {
            Binding.ConfigureBindings(services);
            FactoryAttribute.ConfigureFactories(services);
        }

    }

}