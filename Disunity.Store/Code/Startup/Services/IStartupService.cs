using Microsoft.AspNetCore.Builder;


namespace Disunity.Store.Code.Startup.Services {

    public interface IStartupService {

        void Startup(IApplicationBuilder app);

    }

}