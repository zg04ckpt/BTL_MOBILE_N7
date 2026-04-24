using Models.Users.DTOs;
using Models.Users.Requests;

namespace Feature.Users.Interfaces
{
    public interface IAuthService
    {
        Task<LoginSesssionDto> LogInAsync(LoginRequest request, int? loginLiveTimeMinutes = null);
        Task RegisterAsync(RegisterRequest request);
        Task<LoginSesssionDto> GetLoginInfoAsync(int userId);
    }
}
