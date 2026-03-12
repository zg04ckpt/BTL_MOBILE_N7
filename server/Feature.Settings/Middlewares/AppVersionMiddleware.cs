using CNLib.Services.Logs;
using Feature.Settings.Helpers;
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

        public async Task InvokeAsync(HttpContext context, ISystemConfigurationService configService)
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

            // Check if client version header exists
            if (context.Request.Headers.TryGetValue("App-Version", out var clientVersionHeader))
            {
                var clientVersion = clientVersionHeader.ToString();
                var requiredVersion = await ConfigHelper.GetRequiredAppVersionAsync(configService);

                if (!ConfigHelper.IsVersionAllowed(clientVersion, requiredVersion))
                {
                    _logService.LogInfo($"Version blocked: {clientVersion} < {requiredVersion}");
                    
                    context.Response.StatusCode = StatusCodes.Status426UpgradeRequired;
                    context.Response.ContentType = "application/json";
                    
                    var response = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        isSuccess = false,
                        message = $"App version {clientVersion} is outdated. Please update to version {requiredVersion} or higher.",
                        data = new
                        {
                            clientVersion,
                            requiredVersion,
                            updateRequired = true
                        }
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
