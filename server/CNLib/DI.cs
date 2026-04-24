using CNLib.Services.External.Sheets;
using CNLib.Services.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CNLib
{
    public static class DI
    {
        public static IServiceCollection AddCNLib(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IGgSheetService, GgSheetService>();
            services.Configure<SheetLogConfig>(configuration.GetSection("SheetLogConfig"));
            services.AddSingleton(typeof(ILogService<>), typeof(SheetLogService<>));
            return services;
        }
    }
}
