using CNLib.Api;
using CNLib.Exceptions;
using CNLib.Services.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CNLib.Middlewares
{
    public class GlobalExceptionCatchingMiddleware : IMiddleware
    {
        private readonly ILogService<GlobalExceptionCatchingMiddleware> _logger;

        public GlobalExceptionCatchingMiddleware(
            ILogService<GlobalExceptionCatchingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await WriteResponseAsync(context, ApiResponse.Error(ex.Message));
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await WriteResponseAsync(context, ApiResponse.Error(ex.Message));
            }
            catch (ForbiddenException ex)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await WriteResponseAsync(context, ApiResponse.Error(ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await WriteResponseAsync(context, ApiResponse.Error(ex.Message));
            }
            catch (ServerErrorException ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteResponseAsync(context, ApiResponse.InternalServerError(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unknown error: " + ex.Message, ex.StackTrace);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteResponseAsync(context, ApiResponse.InternalServerError());
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, object response)
        {
            context.Response.ContentType = "application/json";
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }

    public static class GlobalExceptionCatchingMiddlewareExtension
    {
        public static IServiceCollection AddGlobalExceptionCatching(this IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionCatchingMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionCatching(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionCatchingMiddleware>();
        }
    }
}
