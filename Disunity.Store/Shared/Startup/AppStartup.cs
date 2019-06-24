using System;
using System.Dynamic;
using System.Threading.Tasks;
using Disunity.Store.Areas.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disunity.Store.Shared.Startup
{
    public static class AppStartup
    {
        public static void Startup(IConfiguration config,
            IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider services)
        {
            // Register Syncfusion license
//            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(config["Syncfusion:License"]);

            EnvironmentStartup(app, env, services);

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();

            CreateRoles(config, services).Wait();
        }

        public static void DevelopmentStartup(IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider serivces)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
        }

        public static void ProductionStartup(IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider serivces)
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        public static void EnvironmentStartup(IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                DevelopmentStartup(app, env, services);
            }
            else
            {
                ProductionStartup(app, env, services);
            }
        }

        private static async Task CreateRoles(IConfiguration config, IServiceProvider serviceProvider)
        {
            //initializing custom roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserIdentity>>();

            var roleNames = Enum.GetNames(typeof(UserRoles));

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var superuser = new UserIdentity
            {
                UserName = config.GetValue("AdminUser:Name",
                    config.GetValue<string>("AdminUser:Email")),
                Email = config["AdminUser:Email"],
            };

            var pwd = config["AdminUser:Password"];
            if (superuser.Email == null || pwd == null)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
                logger.LogWarning("Skipping creating super user as user was missing email or password");
                return; // super user not full specified, don't create it
            }

            var user = await userManager.FindByEmailAsync(config["AdminUser:Email"]);

            if (user == null)
            {
                var createSuperUser = await userManager.CreateAsync(superuser, pwd);
                if (createSuperUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(superuser, UserRoles.Admin.ToString());
                }
            }
        }
    }
}