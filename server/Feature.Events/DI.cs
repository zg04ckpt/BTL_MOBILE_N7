using Microsoft.Extensions.DependencyInjection;
using Feature.Events.Interfaces;
using Feature.Events.Services;

namespace Feature.Events
{
    public static class DI
    {
        public static IServiceCollection AddEventFeature(this IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();

            return services;
        }
    }
}
