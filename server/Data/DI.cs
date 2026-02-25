using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DI
    {
        public static IServiceCollection AddDBService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_PGDB_CONNECTION_STRING));
            });
            services.AddScoped<DataInitializer>();

            return services;
        }

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DataInitializer>();
            await initializer.CheckAndUpdateFromMigrationAsync();
            await initializer.CheckAndInitDefaultDataAsync();
        }
    }
}
