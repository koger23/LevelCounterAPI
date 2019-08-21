using LevelCounter.Models;
using LevelCounter.Repository;
using LevelCounter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelCounter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            var connectionString = isProduction ? "ProductionConnection" : "DefaultConnection";

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString(connectionString)));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            });
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "http://www.kogero.com",
                        ValidAudience = "http://www.kogero.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["APISecretKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            SeedRoles(roleManager);
            SeedAdminUser(userManager);
            app.UseStaticFiles();
            app.UseMvc();
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new List<string>() { "Admin", "User" };
            foreach (var name in roleNames)
            {
                if (!roleManager.RoleExistsAsync(name).Result)
                {
                    var roleResult = roleManager.CreateAsync(new IdentityRole() { Name = name }).Result;
                }
            }
        }

        private static void SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            var password = "Passw0rd";
            var users = new Dictionary<string, string>
            {
                ["admin@kogero.com"] = "Admin",
            };
            foreach (var userEntry in users)
            {
                if (userManager.FindByEmailAsync(userEntry.Key).Result == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userEntry.Key,
                        Email = userEntry.Key,
                        Statistics = new Statistics()
                    };
                    var result = userManager.CreateAsync(user, password).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, userEntry.Value).Wait();
                    }
                }
            }
        }
    }
}
