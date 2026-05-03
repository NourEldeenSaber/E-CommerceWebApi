using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presistence.Data;
using Presistence.Identity;
using Presistence.Repositories;
using Shared.Common;
using StackExchange.Redis;
using System.Text;

namespace E_Commerce.API.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration) 
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<IdentityStoreDbContext>();

            services.ValidateJwt(configuration);

            return services;
        }
    
        public static IServiceCollection ValidateJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwrOptions = configuration.GetSection("JWTOptions").Get<JwtOptions>(); //map
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwrOptions!.Issuer,
                    ValidAudience = jwrOptions.Audiance,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwrOptions.SecretKey))

                };
            });
            services.AddAuthorization();
            
            return services;
        }
    }
}
