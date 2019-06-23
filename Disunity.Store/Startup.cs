using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Disunity.Store.Models;
using Microsoft.Extensions.Logging;
using Disunity.Store.Areas.Identity.Models;

namespace Disunity.Store
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      ServicesStartup.ConfigureServices(services, Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseAuthentication();

      app.UseMvc();

      CreateRoles(serviceProvider).Wait();
    }

    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
      //initializing custom roles
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var UserManager = serviceProvider.GetRequiredService<UserManager<UserIdentity>>();

      string[] roleNames = Enum.GetNames(typeof(UserRoles));
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
          //create the roles and seed them to the database
          roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
      }

      var superuser = new UserIdentity
      {
        UserName = Configuration.GetValue("AdminUser:Name",
                   Configuration.GetValue<string>("AdminUser:Email")),
        Email = Configuration["AdminUser:Email"],
      };

      string pwd = Configuration["AdminUser:Password"];
      if (superuser.Email == null || pwd == null)
      {
        var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
        logger.LogWarning("Skipping creating super user as user was missing email or password");
        return; // super user not full specified, don't create it
      }

      var _user = await UserManager.FindByEmailAsync(Configuration["AdminUser:Email"]);

      if (_user == null)
      {
        var createSuperUser = await UserManager.CreateAsync(superuser, pwd);
        if (createSuperUser.Succeeded)
        {
          await UserManager.AddToRoleAsync(superuser, UserRoles.Admin.ToString());
        }
      }
    }
  }
}
