using CNLib.Services.Logs;
using Feature.Settings.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Feature.Settings.Middlewares
{
    public class AppVersionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService<AppVersionMiddleware> _logService;

        public AppVersionMiddleware(RequestDelegate next, ILogService<AppVersionMiddleware> logService)
        {
            _next = next;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext context, ISettingsService configService)
        {
            // Skip version check for certain paths
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            if (path.StartsWith("/swagger") || 
                path.StartsWith("/health") ||
                path.StartsWith("/api/systemconfigurations"))
            {
                await _next(context);
                return;
            }

            await _next(context);
        }
    }
}
