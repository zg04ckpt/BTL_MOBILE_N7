using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CNLib.Middlewares
{
    public class ExtractJwtTokenFromCookieMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Cookies["aToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Authorization = "Bearer " + token;
            }
            await next(context);
        }
    }

    public static class ExtractJwtTokenFromCookieMiddlewareExtension
    {
        public static IServiceCollection AddExtractJwtTokenFromCookie(this IServiceCollection services)
        {
            services.AddTransient<ExtractJwtTokenFromCookieMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseExtractJwtTokenFromCookie(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExtractJwtTokenFromCookieMiddleware>();
        }
    }
}
