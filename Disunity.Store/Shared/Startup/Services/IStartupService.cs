using Microsoft.AspNetCore.Builder;


namespace Disunity.Store.Shared.Startup.Services {

    public interface IStartupService {

        void Startup(IApplicationBuilder app);

    }

}