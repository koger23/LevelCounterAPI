using AutoMapper;
using LevelCounter.Models;
using LevelCounter.Repository;
using LevelCounter.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace LevelCounter.Configs
{
    public static class ServiceConfigurations
    {
        public static object Configuration { get; private set; }

        public static IServiceCollection AddDbContextSetup(this IServiceCollection services, IConfiguration config)
        {

            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            var connectionString = isProduction ? "ProductionConnection" : "DefaultConnection";

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString(connectionString)));
            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            });
            return services;
        }

        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "http://www.kogero.com",
                        ValidAudience = "http://www.kogero.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["APISecretKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IRelationshipService, RelationshipService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
        {
            services.AddMvc()
                   .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                   .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAutoMapper();
            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = MappingProfiles.GetAutoMapperProfiles().CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
