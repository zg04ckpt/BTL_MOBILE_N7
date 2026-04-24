using CNLib.Services.Logs;
using Core.Models;
using Feature.Settings.Interfaces;
using Feature.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users.Requests;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISettingsService _configService;
        private readonly ILogService<AuthController> _logService;

        public AuthController(
            IAuthService authService, 
            ISettingsService configService,
            ILogService<AuthController> logService)
        {
            _authService = authService;
            _configService = configService;
            _logService = logService;
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetLoginInfo()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var info = await _authService.GetLoginInfoAsync(userId);
            return Ok(ApiResponse.Success(info));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest request)
        {
            var settings = await _configService.GetAllSettingsAsync();
            var loginLiveTime = settings.LoginLiveTime;
            var loginRes = await _authService.LogInAsync(request, loginLiveTime);

            Response.Cookies.Append("aToken", loginRes.AccessToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                MaxAge = TimeSpan.FromMinutes(loginLiveTime)
            });
            
            return Ok(ApiResponse.Success(loginRes));
        }



        [HttpDelete("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("aToken", new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Ok(ApiResponse.Success());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(request);
            return Ok(ApiResponse.Success());
        }
    }
}
