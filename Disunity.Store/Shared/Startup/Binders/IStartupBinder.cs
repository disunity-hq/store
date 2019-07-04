using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Shared.Startup.Binders {

    public interface IStartupBinder {

        void Bind(IServiceCollection services);

    }

}