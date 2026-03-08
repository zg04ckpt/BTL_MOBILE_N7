using Core.Models;
using Feature.Users.Interfaces;
using Feature.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(ApiResponse.Success(users));
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] SearchUserRequest request)
        {
            var result = await _userService.GetPagingAsync(request);
            return Ok(ApiResponse.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            
            if (user == null)
            {
                return NotFound(ApiResponse.Failure("Người dùng không tồn tại"));
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


        [HttpPost] 
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request);
            return Ok(ApiResponse.Success("Tạo người dùng thành công", result));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateUserRequest request)
        {
            var result = await _userService.UpdateAsync(id, request);
            return Ok(ApiResponse.Success("Cập nhật thông tin thành công", result));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return Ok(ApiResponse.Success("Xóa người dùng thành công", result));
        }
    }
}
