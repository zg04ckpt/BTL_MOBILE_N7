using CNLib.Services.Logs;
using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Interfaces;
using Models.Users.DTOs;
using Models.Users.Enums;
using Models.Users.Requests;
using System.Linq.Expressions;

namespace Feature.Users.Services
{
    public class UserService
        : CrudWithPagingService<User, CreateUserRequest, UpdateUserRequest, UserListItemDto, UserDetailDto, SearchUserRequest>, IUserService
    {
        private readonly IStorageService _storageService;
        private readonly ILogService<UserService> _logService;

        public UserService(IUnitOfWork uow, IStorageService storageService, ILogService<UserService> logService) : base(uow)
        {
            _storageService = storageService;
            _logService = logService;
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var userRepository = _uow.Repository<User>();

            var user = await userRepository.GetFirstAsync(
                predicate: u => u.Id == userId
            );

            if (user == null)
            {
                _logService.LogError($"Profile not found: #{userId}");
                throw new NotFoundException("Người dùng không tồn tại");
            }

            //var userInMatchHistory = await _uow.Repository<>
            
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

        public async Task<ChangedResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request)
        {
            var userRepository = _uow.Repository<User>();
            
            var user = await userRepository.GetFirstAsync(u => u.Id == userId);
            
            if (user == null)
            {
                _logService.LogError($"Update profile failed: User #{userId} not found");
                throw new NotFoundException("Người dùng không tồn tại");
            }

            if (user.Name != request.Name 
                && await userRepository.ExistsAsync(u => u.Id != userId && u.Name == request.Name))
            {
                _logService.LogError($"Update profile failed: Name exists - {request.Name}");
                throw new BadRequestException("Tên hiển thị đã được sử dụng");
            }

            user.Name = request.Name;

            if (request.Avatar != null)
            {
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    await _storageService.DeleteAsync(user.AvatarUrl);
                }

                user.AvatarUrl = await _storageService.SaveAsync(request.Avatar);
            }

            await userRepository.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            _logService.LogSuccess($"Profile updated: #{userId}");

            return new ChangedResponse
            {
                Id = user.Id
            };
        }

        public async override Task<IEnumerable<UserListItemDto>> GetAllAsync()
        {
            return await _uow.Repository<User>().GetAllAsync(
                predicate: e => true,
                selector: e => new UserListItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    AvatarUrl = e.AvatarUrl,
                    Status = e.Status,
                    RoleName = e.Role.Name
                });
        }

        public override async Task<Paginated<UserListItemDto>> GetPagingAsync(SearchUserRequest request)
        {
            var entities = await _uow.Repository<User>().GetAllAsync(
                predicate: GetPagingFilter(request),
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: GetOrderBy(request.OrderBy),
                asc: request.IsAsc,
                selector: e => new UserListItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    AvatarUrl = e.AvatarUrl,
                    Status = e.Status,
                    RoleName = e.Role.Name
                });

            var totalItems = await _uow.Repository<User>().CountAsync(
                predicate: GetPagingFilter(request));

            return new Paginated<UserListItemDto>
            {
                Items = entities,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems
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
                {
                    _logService.LogError($"Create user failed: Email exists - {request.Email}");
                    throw new BadRequestException("Email đã được sử dụng");
                }

                if (existedUser.PhoneNumber == request.PhoneNumber)
                {
                    _logService.LogError($"Create user failed: Phone exists - {request.PhoneNumber}");
                    throw new BadRequestException("Số điện thoại đã được sử dụng");
                }

                if (existedUser.Name == request.Name)
                {
                    _logService.LogError($"Create user failed: Name exists - {request.Name}");
                    throw new BadRequestException("Tên hiển thị đã được sử dụng");
                }
            }
        }

        protected override async Task ConfirmValidUpdateDataAsync(User entity, UpdateUserRequest request)
        {
            var userRepository = _uow.Repository<User>();

            if (entity.Name != request.Name 
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.Name == request.Name))
            {
                _logService.LogError($"Update user failed: Name exists - {request.Name}");
                throw new BadRequestException("Tên hiển thị đã được sử dụng");
            }

            if (entity.Email != request.Email
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.Email == request.Email))
            {
                _logService.LogError($"Update user failed: Email exists - {request.Email}");
                throw new BadRequestException("Email đã được sử dụng");
            }

            if (entity.PhoneNumber != request.PhoneNumber
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.PhoneNumber == request.PhoneNumber))
            {
                _logService.LogError($"Update user failed: Phone exists - {request.PhoneNumber}");
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

        protected override Task<User> MapFromCreateToEntityAsync(CreateUserRequest request)
        {
            var user = new User
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

            return Task.FromResult(user);
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

        protected override async Task UpdateEntityAsync(User entity, UpdateUserRequest request)
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

            if (request.Avatar != null)
            {
                if (!string.IsNullOrEmpty(entity.AvatarUrl))
                {
                    await _storageService.DeleteAsync(entity.AvatarUrl);
                }

                entity.AvatarUrl = await _storageService.SaveAsync(request.Avatar);
            }
        }
    }
}
