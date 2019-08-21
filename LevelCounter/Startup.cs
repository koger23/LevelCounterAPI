using LevelCounter.Configs;
using LevelCounter.Models;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LevelCounter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextSetup(Configuration);
            services.AddIdentityConfiguration();
            services.AddAuthenticationConfiguration(Configuration);
            services.RegisterServices();
            services.AddMvcConfiguration();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationContext applicationContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                applicationContext.Database.Migrate();
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            DataSeed.SeedDefaultDatas(roleManager, userManager);
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
