using Feature.Settings.Interfaces;
using Feature.Settings.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Settings
{
    public static class DI
    {
        public static IServiceCollection AddSettingsFeature(this IServiceCollection services)
        {
            services.AddScoped<ISystemConfigurationService, SystemConfigurationService>();
            
            return services;
        }
    }
}
