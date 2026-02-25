
namespace API.Middlewares
{
    public class ExtractTokenMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Cookies["aToken"];
            if (!string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Cookie: " + token);
                context.Request.Headers.Authorization = "Bearer " + token;
            }
            await next(context);
        }
    }
}
