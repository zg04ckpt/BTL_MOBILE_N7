using Feature.Overview.Interfaces;
using Feature.Overview.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Overview
{
    public static class DI
    {
        public static IServiceCollection AddOverviewFeature(this IServiceCollection services)
        {
            services.AddScoped<IAnalyticsService, AnalyticsService>();
            services.AddScoped<IRankService, RankService>();
            
            return services;
        }
    }
}
