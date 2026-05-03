using Services;
using Services.Implementations;
using Sevices.Abstraction.Contracts;
using Shared.Common;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();
            services.Configure<JwtOptions>(configuration.GetSection("JWTOptions"));
            return services;
        }
    }
}
