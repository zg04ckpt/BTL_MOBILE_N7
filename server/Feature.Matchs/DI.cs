using Feature.Matchs.Interfaces;
using Feature.Matchs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Matchs
{
    public static class DI
    {
        public static IServiceCollection AddMatchFeature(this IServiceCollection services)
        {
            services.AddSingleton<IMatchRealtimeService, FirebaseService>();
            services.AddScoped<ILobbyService, LobbyService>();
            services.AddScoped<IMatchService, MatchService>();

            return services;
        }
    }
}
