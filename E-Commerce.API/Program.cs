using E_Commerce.API.Extensions;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region DI Container
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Web Api Service
            builder.Services.AddWebApiServices();

            //Infrastructure Services
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //Core Services
            builder.Services.AddCoreServices(builder.Configuration);

            #endregion

            #region Piplines - Middlewares
            
            var app = builder.Build();

            await app.SeedDatabaseAsync();

            //Handle Exception
            app.UseExceptionHandlingMiddlewares();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run(); 

            #endregion
        }
    }
}
