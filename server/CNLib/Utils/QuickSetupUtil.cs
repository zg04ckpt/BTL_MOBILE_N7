using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace CNLib.Utils
{
    public static class QuickSetupUtil
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddRouting(opt => opt.LowercaseUrls = true);
        }

        public static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),              // /api/v1/users
                    new QueryStringApiVersionReader("api-version"),     // ?api-version=1.0
                    new HeaderApiVersionReader("x-api-version")       // x-api-version: 1.0
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
