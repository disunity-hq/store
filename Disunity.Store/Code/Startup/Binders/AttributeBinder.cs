using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Code.Startup.Binders {

    public class AttributeBinder : IStartupBinder {

        public static void Bind(IServiceCollection services, IConfiguration config) {
            Binding.ConfigureBindings(services);
            FactoryAttribute.ConfigureFactories(services);
        }

    }

}