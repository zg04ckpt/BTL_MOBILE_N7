using Core.Models;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest request)
        {
            var loginRes = await _authService.LogInAsync(request);

            Response.Cookies.Append("aToken", loginRes.AccessToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                MaxAge = TimeSpan.FromMinutes(25200)
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
