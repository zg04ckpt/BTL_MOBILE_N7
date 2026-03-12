using CNLib.Services.Logs;
using Feature.Settings.Helpers;
using Feature.Settings.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Feature.Settings.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService<MaintenanceMiddleware> _logService;

        public MaintenanceMiddleware(RequestDelegate next, ILogService<MaintenanceMiddleware> logService)
        {
            _next = next;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext context, ISystemConfigurationService configService)
        {
            // Skip maintenance check for certain paths
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            if (path.StartsWith("/api/auth") || 
                path.StartsWith("/api/systemconfigurations") ||
                path.StartsWith("/swagger") ||
                path.StartsWith("/health"))
            {
                await _next(context);
                return;
            }

            var isMaintenanceMode = await ConfigHelper.IsMaintenanceModeAsync(configService);

            if (isMaintenanceMode)
            {
                var clientIP = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var whitelistIPs = await ConfigHelper.GetWhitelistIPsAsync(configService);

                if (!whitelistIPs.Contains(clientIP) && clientIP != "::1" && clientIP != "127.0.0.1")
                {
                    _logService.LogInfo($"Maintenance: Blocked {clientIP} -> {path}");
                    
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    context.Response.ContentType = "application/json";
                    
                    var response = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        isSuccess = false,
                        message = "System is under maintenance. Please try again later.",
                        data = (object?)null
                    }, new System.Text.Json.JsonSerializerOptions 
                    { 
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase 
                    });
                    
                    await context.Response.WriteAsync(response);
                    return;
                }
            }

            await _next(context);
        }
    }
}
