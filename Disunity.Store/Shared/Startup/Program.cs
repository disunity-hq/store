using System.Linq;
using System.Reflection;

using Disunity.Store.Shared.Startup.Binders;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Shared.Startup
{

    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseWebRoot("../Frontend/dist")
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              config.AddEnvironmentVariables();
                              config.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
                          })
                          .ConfigureLogging(f => f.AddConsole().AddDebug())
                          .UseStartup<Startup>();
        }

    }

}