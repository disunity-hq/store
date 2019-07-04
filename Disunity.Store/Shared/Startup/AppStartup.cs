using System;

using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Syncfusion.Licensing;


namespace Disunity.Store.Shared.Startup {

    public class AppStartup {

        private readonly IConfiguration _config;
        private readonly ILogger<AppStartup> _logger;
        private readonly IApplicationBuilder _app;
        private readonly IHostingEnvironment _env;
        private readonly IServiceProvider _services;

        public AppStartup(IConfiguration config,
                          ILogger<AppStartup> logger,
                          IApplicationBuilder app,
                          IHostingEnvironment env,
                          IServiceProvider services) {
            _config = config;
            _logger = logger;
            _app = app;
            _env = env;
            _services = services;
            
        }

        public void Startup() {
            SyncfusionLicenseProvider.RegisterLicense(_config["Syncfusion:License"]);

            DatabaseStartup();
            EnvironmentStartup();

            _app.UseStaticFiles();
            _app.UseCookiePolicy();
            _app.UseAuthentication();

            _app.UseMvc(routes => {
                routes.MapAreaRoute("api", "API",
                                    "api/v{version:apiVersion}/[controller]/[action=Index]");
            });
            
        }

        public void DevelopmentStartup() {
            _app.UseDeveloperExceptionPage();
            _app.UseDatabaseErrorPage();
        }

        public void ProductionStartup() {
            _app.UseHttpsRedirection();
            _app.UseExceptionHandler("/Error");
            _app.UseHsts(); // see https://aka.ms/aspnetcore-hsts.
        }

        public void EnvironmentStartup() {
            if (_env.IsDevelopment()) {
                DevelopmentStartup();
            } else {
                ProductionStartup();
            }
        }

        public void DatabaseStartup() {
            var dbContext = _services.GetRequiredService<ApplicationDbContext>();

            try {
                if (_config.GetValue("Database.MigrateOnStartup", _env.IsDevelopment())) {
                    dbContext.Database.Migrate();
                }

                if (_config.GetValue("Database.SeedOnStartup", _env.IsDevelopment())) {
                    _services.GetRequiredService<DbSeeder>();
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

    }

}