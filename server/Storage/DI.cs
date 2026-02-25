using Microsoft.Extensions.DependencyInjection;
using Storage.Services;

namespace Storage
{
    public static class DI
    {
        public static IServiceCollection AddStorageServices(this IServiceCollection services)
        {
            services.AddScoped<IImageStorageService, ImageStorageService>();
            services.AddHostedService<ImageCleanupService>();
            
            return services;
        }
    }
}
