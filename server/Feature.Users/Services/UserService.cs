using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Enums;
using Feature.Users.Interfaces;
using Feature.Users.Models;
using System.Linq.Expressions;

namespace Feature.Users.Services
{
    public class UserService
        : CrudWithPagingService<User, CreateUserRequest, UpdateUserRequest, UserListItemDto, UserDetailDto, SearchUserRequest>, IUserService
    {
        public UserService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var userRepository = _uow.Repository<User>();

            var user = await userRepository.GetFirstAsync(
                predicate: u => u.Id == userId
            );

            if (user == null)
            {
                throw new NotFoundException("Người dùng không tồn tại");
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                AvatarUrl = user.AvatarUrl,
                Level = user.Level,
                Rank = user.Rank,
                RankScore = user.RankScore,
                WinningStreak = 0,
                WinningRate = 0
            };
        }

        protected override async Task ConfirmValidCreateDataAsync(CreateUserRequest request)
        {
            var userRepository = _uow.Repository<User>();

            var existedUser = await userRepository.GetFirstAsync(
                    predicate: u => u.Email == request.Email
                                || u.PhoneNumber == request.PhoneNumber
                                || u.Name == request.Name,
                    selector: u => new { u.Email, u.PhoneNumber, u.Name }
                );

            if (existedUser != null)
            {
                if (existedUser.Email == request.Email)
                    throw new BadRequestException("Email đã được sử dụng");

                if (existedUser.PhoneNumber == request.PhoneNumber)
                    throw new BadRequestException("Số điện thoại đã được sử dụng");

                if (existedUser.Name == request.Name)
                    throw new BadRequestException("Tên hiển thị đã được sử dụng");
            }
        }

        protected override async Task ConfirmValidUpdateDataAsync(User entity, UpdateUserRequest request)
        {
            var userRepository = _uow.Repository<User>();

            if (entity.Name != request.Name 
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.Name == request.Name))
            {
                throw new BadRequestException("Tên hiển thị đã được sử dụng");
            }

            if (entity.Email != request.Email
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.Email == request.Email))
            {
                throw new BadRequestException("Email đã được sử dụng");
            }

            if (entity.PhoneNumber != request.PhoneNumber
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.PhoneNumber == request.PhoneNumber))
            {
                throw new BadRequestException("Số điện thoại đã được sử dụng");
            }
        }

        protected override Expression<Func<User, object>> GetOrderBy(string? orderBy)
        {
            return orderBy?.ToLower() switch
            {
                "name" => u => u.Name,
                "email" => u => u.Email,
                "createdat" => u => u.CreatedAt,
                "level" => u => u.Level,
                "rank" => u => u.Rank,
                _ => u => u.Id
            };
        }

        protected override Expression<Func<User, bool>> GetPagingFilter(SearchUserRequest request)
        {
            return u => 
                (string.IsNullOrEmpty(request.Email) || u.Email.Contains(request.Email)) &&
                (string.IsNullOrEmpty(request.Phone) || u.PhoneNumber.Contains(request.Phone)) &&
                (string.IsNullOrEmpty(request.Name) || u.Name.Contains(request.Name));
        }

        protected override User MapFromCreateToEntity(CreateUserRequest request)
        {
            return new User
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Status = AccountStatus.Active,
                Level = 1,
                Rank = 0,
                RankScore = 0,
                Exp = 0,
                CreatedAt = DateTime.UtcNow,
                RoleId = request.RoleId,
                AvatarUrl = string.Empty
            };
        }

        protected override UserDetailDto MapFromEntityToDetail(User entity)
        {
            return new UserDetailDto
            {
                Id = entity.Id,
                AvatarUrl = entity.AvatarUrl,
                PhoneNumber = entity.PhoneNumber,
                Name = entity.Name,
                Email = entity.Email,
                Status = entity.Status,
                Level = entity.Level,
                Rank = entity.Rank,
                RankScore = entity.RankScore,
                Exp = entity.Exp,
                CreatedAt = entity.CreatedAt,
                RoleId = entity.RoleId,
                RoleName = entity.Role?.Name ?? string.Empty
            };
        }

        protected override UserListItemDto MapFromEntityToListItem(User entity)
        {
            return new UserListItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                AvatarUrl = entity.AvatarUrl,
                Status = entity.Status,
                RoleName = entity.Role?.Name ?? string.Empty
            };
        }

        protected override Task UpdateEntityAsync(User entity, UpdateUserRequest request)
        {
            entity.Name = request.Name;
            entity.Email = request.Email;
            entity.PhoneNumber = request.PhoneNumber;
            entity.Status = request.Status;
            entity.Level = request.Level;
            entity.Rank = request.Rank;
            entity.RankScore = request.RankScore;
            entity.Exp = request.Exp;
            entity.RoleId = request.RoleId;

            // TODO: Xử lý upload avatar từ request.Avatar
            // entity.AvatarUrl = ...

            return Task.CompletedTask;
        }
    }
}
