using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Shared.Startup.Binders {

    public class AttributeBinder : IStartupBinder {

        private readonly ILogger<AttributeBinder> _logger;

        public AttributeBinder(ILogger<AttributeBinder> logger) {
            _logger = logger;

        }

        public void Bind(IServiceCollection services) {
            Binding.ConfigureBindings(services);
            FactoryAttribute.ConfigureFactories(services);
        }

    }

}