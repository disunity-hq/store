using System;

using Disunity.Store.Code.Startup;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Code.Extensions {

    public static class ServiceCollectionExt {

        public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService {
            services.AddTransient<TService, TImplementation>();
            services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
            services.AddSingleton<IFactory<TService>, Factory<TService>>();
        }

    }

}