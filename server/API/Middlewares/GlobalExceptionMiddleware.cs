
using Core.Exceptions;
using Core.Models;
using Core.Utilities;
using System.Text.Json;

namespace API.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (ServerErrorException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error: {Message}", ex.Message);
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, StringUtil.ApiMessages.UnknownError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            
            var response = ApiResponse.Failure(message);
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
