using LevelCounter.Configs;
using LevelCounter.Hubs;
using LevelCounter.Models;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
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
            services.AddSwaggerDoc();
            services.RegisterServices();
            services.AddMvcConfiguration();
            services.AddSignalR();
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
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                }); 
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            DataSeed.SeedDefaultDatas(roleManager, userManager);
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            app.UseFileServer();
            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/game");
            });
            app.UseMvc();
        }
    }
}
