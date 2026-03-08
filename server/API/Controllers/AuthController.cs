using CNLib.Services.Logs;
using Core.Models;
using Feature.Settings.Helpers;
using Feature.Settings.Interfaces;
using Feature.Users.Interfaces;
using Feature.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISystemConfigurationService _configService;
        private readonly ILogService<AuthController> _logService;

        public AuthController(
            IAuthService authService, 
            ISystemConfigurationService configService,
            ILogService<AuthController> logService)
        {
            _authService = authService;
            _configService = configService;
            _logService = logService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest request)
        {
            var loginLiveTime = await ConfigHelper.GetLoginLiveTimeAsync(_configService);
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(request);
            return Ok(ApiResponse.Success());
        }
    }
}
