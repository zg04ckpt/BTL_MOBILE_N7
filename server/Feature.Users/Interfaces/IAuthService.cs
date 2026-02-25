using Feature.Users.Models;

namespace Feature.Users.Interfaces
{
    public interface IAuthService
    {
        Task<LoginSesssionDto> LogInAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
    }
}
