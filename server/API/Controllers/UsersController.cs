using CNLib.Services.Logs;
using CNLib.Utils;
using Core.Models;
using Core.Utilities;
using Feature.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users.Requests;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService<UsersController> _logService;

        public UsersController(IUserService userService, ILogService<UsersController> logService)
        {
            _userService = userService;
            _logService = logService;
        }

        [HttpGet]
        [Authorize(Policy = Core.Utilities.StringUtil.PolicyNames.OnlyAdmin)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(ApiResponse.Success(users));
        }

        [HttpGet("paging")]
        [Authorize(Policy = Core.Utilities.StringUtil.PolicyNames.OnlyAdmin)]
        public async Task<IActionResult> GetPaging([FromQuery] SearchUserRequest request)
        {
            var result = await _userService.GetPagingAsync(request);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Core.Utilities.StringUtil.PolicyNames.OnlyAdmin)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            
            if (user == null)
            {
                _logService.LogError($"[API] User not found: #{id}");
                return NotFound(ApiResponse.Failure("User not found"));
            }

            return Ok(ApiResponse.Success(user));
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _userService.GetProfileAsync(id);
            return Ok(ApiResponse.Success(profile));
        }

        [HttpPut("profile")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _userService.UpdateProfileAsync(id, request);
            return Ok(ApiResponse.Success("Profile updated successfully", result));
        }

        [HttpPost]
        [Authorize(Policy = Core.Utilities.StringUtil.PolicyNames.OnlySuperAdmin)] 
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request);
            return Ok(ApiResponse.Success("User created successfully", result));
        }


        [HttpPut("{id}")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
        [Authorize(Policy = Core.Utilities.StringUtil.PolicyNames.OnlyAdmin)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateUserRequest request)
        {
            var result = await _userService.UpdateAsync(id, request);
            return Ok(ApiResponse.Success("User updated successfully", result));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = Core.Utilities.StringUtil.PolicyNames.OnlySuperAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return Ok(ApiResponse.Success("User deleted successfully", result));
        }
    }
}
