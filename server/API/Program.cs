using API.Middlewares;
using Core.Exceptions;
using Core.Models;
using Core.Utilities;
using Data;
using Feature.Overview;
using Feature.Quizzes;
using Feature.Settings;
using Feature.Users;
using Feature.Users.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Storage;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddUsersFeature();
            builder.Services.AddQuizzesFeature();
            builder.Services.AddSettingsFeature();
            builder.Services.AddOverviewFeature();
            builder.Services.AddDBService();
            builder.Services.AddStorageServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocal",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
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
                    p.RequireRole(nameof(RoleName.Admin));
                });
            });
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_SECRET_KEY)))
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
                                throw new UnauthorizedException("Token hết hạn");
                            }
                            throw new UnauthorizedException("Token không hợp lệ");
                        }
                        
                        throw new UnauthorizedException("Yêu cầu xác thực");
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

                        var response = ApiResponse.Failure("Dữ liệu không hợp lệ", errors);
                        return new BadRequestObjectResult(response);
                    };
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<ExtractTokenMiddleware>();

            // Optional: Enable maintenance mode and version check
            app.UseMiddleware<Feature.Settings.Middlewares.MaintenanceMiddleware>();
            app.UseMiddleware<Feature.Settings.Middlewares.AppVersionMiddleware>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(AppContext.BaseDirectory, "uploads")),
                RequestPath = "/uploads"
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors("AllowLocal");
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            await app.InitializeDatabaseAsync();

            app.Run();
        }
    }
}
