using API.Middlewares;
using CNLib;
using Core.Exceptions;
using Core.Models;
using Core.Utilities;
using Data;
using Feature.Events;
using Feature.Matchs;
using Feature.Overview;
using Feature.Quizzes;
using Feature.Settings;
using Feature.Settings.Middlewares;
using Feature.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Models.Users.Enums;
using Storage;
using System.Text;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ValidateJwtConfiguration(builder.Configuration);

            builder.Services.AddUsersFeature();
            builder.Services.AddQuizzesFeature();
            builder.Services.AddSettingsFeature();
            builder.Services.AddOverviewFeature();
            builder.Services.AddMatchFeature();
            builder.Services.AddEventFeature();
            builder.Services.AddDBService();
            builder.Services.AddStorageServices();
            builder.Services.AddCNLib(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebLocalTest",
                    builder => builder
                        .WithOrigins("http://localhost:3000")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                options.AddPolicy("AllowTool",
                    builder => builder
                        .WithOrigins("https://tool.hoangcn.com")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
            builder.Services.AddTransient<GlobalExceptionMiddleware>();
            builder.Services.AddTransient<ExtractTokenMiddleware>();
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy(StringUtil.PolicyNames.OnlyAdmin, p =>
                {
                    p.RequireRole(
                        nameof(RoleName.Admin),
                        nameof(RoleName.SuperAdmin)
                    );
                });
                opt.AddPolicy(StringUtil.PolicyNames.OnlySuperAdmin, p =>
                {
                    p.RequireRole(nameof(RoleName.SuperAdmin));
                });
            });
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? "QuizBattle.API";
                var jwtAudience = builder.Configuration["JWT:Audience"] ?? "QuizBattle.Client";
                var secretKey = EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_SECRET_KEY);

                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new InvalidOperationException("JWT Secret Key is not configured. Please set QUIZBATTLE_SECRET_KEY environment variable.");
                }

                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        
                        if (context.AuthenticateFailure != null)
                        {
                            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                            {
                                throw new UnauthorizedException("Token has expired");
                            }
                            throw new UnauthorizedException("Invalid token");
                        }
                        
                        throw new UnauthorizedException("Authentication required");
                    }
                };
            });

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                            );

                        var response = ApiResponse.Failure(errors.First().Value.First(), errors);
                        return new BadRequestObjectResult(response);
                    };
                });

            // Configure multipart body length limit for file uploads
            var maxFileSizeMB = builder.Configuration.GetValue<int>("FileUpload:MaxFileSizeMB", 10);
            var maxFileSizeBytes = maxFileSizeMB * 1024 * 1024;

            builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = maxFileSizeBytes;
                options.ValueLengthLimit = maxFileSizeBytes;
                options.MultipartHeadersLengthLimit = maxFileSizeBytes;
            });

            // Configure Kestrel server limits
            builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = maxFileSizeBytes;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<ExtractTokenMiddleware>();

            app.UseMiddleware<MaintenanceMiddleware>();
            app.UseMiddleware<AppVersionMiddleware>();

            var path = Path.Combine(AppContext.BaseDirectory, "uploads");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/uploads"
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors("AllowWebLocalTest");
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            await app.InitializeDatabaseAsync();

            app.Run();
        }

        private static void ValidateJwtConfiguration(IConfiguration configuration)
        {
            var secretKey = EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_SECRET_KEY);
            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException(
                    "JWT Secret Key not found. Please set QUIZBATTLE_SECRET_KEY environment variable.");
            }

            if (secretKey.Length < 32)
            {
                throw new InvalidOperationException(
                    "JWT Secret Key must be at least 32 characters long for security.");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                Console.WriteLine("WARNING: JWT:Issuer not configured in appsettings. Using default: QuizBattle.API");
            }

            if (string.IsNullOrEmpty(audience))
            {
                Console.WriteLine("WARNING: JWT:Audience not configured in appsettings. Using default: QuizBattle.Client");
            }
        }
    }
}
