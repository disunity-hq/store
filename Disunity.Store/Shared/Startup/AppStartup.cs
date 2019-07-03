using System;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Syncfusion.Licensing;


namespace Disunity.Store.Shared.Startup {

    public static class AppStartup {

        public static void Startup(IConfiguration config,
                                   IApplicationBuilder app,
                                   IHostingEnvironment env,
                                   IServiceProvider services) {
            // Register Syncfusion license
            SyncfusionLicenseProvider.RegisterLicense(config["Syncfusion:License"]);

            DatabaseStartup(config, env, services);

            EnvironmentStartup(app, env, services);

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes => {
                routes.MapAreaRoute("api", "API", "api/v{version:apiVersion}/[controller]/[action=Index]");
            });

            CreateRoles(config, services).Wait();
        }

        public static void DevelopmentStartup(IApplicationBuilder app,
                                              IHostingEnvironment env,
                                              IServiceProvider serivces) {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
        }

        public static void ProductionStartup(IApplicationBuilder app,
                                             IHostingEnvironment env,
                                             IServiceProvider serivces) {
            app.UseHttpsRedirection();
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        public static void EnvironmentStartup(IApplicationBuilder app,
                                              IHostingEnvironment env,
                                              IServiceProvider services) {
            if (env.IsDevelopment()) {
                DevelopmentStartup(app, env, services);
            } else {
                ProductionStartup(app, env, services);
            }
        }

        public static void DatabaseStartup(IConfiguration config,
                                           IHostingEnvironment env,
                                           IServiceProvider services) {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            try {
                if (config.GetValue("Database.MigrateOnStartup", env.IsDevelopment())) {
                    dbContext.Database.Migrate();
                }

                services.GetRequiredService<SeedData>();
            }
            catch (Exception ex) {
                var logger = services.GetRequiredService<ILogger<Startup>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        private static async Task CreateRoles(IConfiguration config, IServiceProvider serviceProvider) {
            //initializing custom roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserIdentity>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();

            var roleNames = Enum.GetNames(typeof(UserRoles));

            foreach (var roleName in roleNames) {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist) {
                    //create the roles and seed them to the database
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    logger.LogInformation($"Created role: {roleName}");
                }
            }

            var superuser = new UserIdentity {
                UserName = config.GetValue("AdminUser:Name",
                                           config.GetValue<string>("AdminUser:Email")),
                Email = config["AdminUser:Email"],
            };

            var pwd = config["AdminUser:Password"];

            if (superuser.Email == null || pwd == null) {
                logger.LogWarning("Skipping creating super user as user was missing email or password");
                return; // super user not full specified, don't create it
            }

            var user = await userManager.FindByEmailAsync(config["AdminUser:Email"]);

            if (user == null) {
                var createSuperUser = await userManager.CreateAsync(superuser, pwd);

                if (createSuperUser.Succeeded) {
                    logger.LogInformation($"Successfully created admin user: {superuser.Email}");

                    var addedRole = await userManager.AddToRoleAsync(superuser, UserRoles.Admin.ToString());

                    if (addedRole.Succeeded) {
                        logger.LogInformation($"Successfully set admin role: {superuser.Email}");
                    }
                }
            } else {
                logger.LogWarning($"Skipping creating super user as email is already taken: {superuser.Email}");
            }
        }

    }

}