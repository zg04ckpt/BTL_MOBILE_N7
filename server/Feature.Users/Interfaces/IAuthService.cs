using Feature.Users.Models;

namespace Feature.Users.Interfaces
{
    public interface IAuthService
    {
        Task<LoginSesssionDto> LogInAsync(LoginRequest request, int? loginLiveTimeMinutes = null);
        Task RegisterAsync(RegisterRequest request);
    }
}
