using Core.Interfaces;
using Core.Models;
using Feature.Users.Entities;
using Feature.Users.Models;

namespace Feature.Users.Interfaces
{
    public interface IUserService
        : ICrudWithPagingService<User, CreateUserRequest, UpdateUserRequest, UserListItemDto, UserDetailDto, SearchUserRequest>
    {
        Task<UserProfileDto> GetProfileAsync(int userId);
        Task<ChangedResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request);
    }
}
