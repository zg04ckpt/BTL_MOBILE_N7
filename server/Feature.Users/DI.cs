using Feature.Users.Interfaces;
using Feature.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Users
{
    public static class DI
    {
        public static IServiceCollection AddUsersFeature(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
