using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Feature.Overview.Interfaces;
using Feature.Users.Entities;
using Feature.Users.Interfaces;
using Feature.Users.Utils;
using LinqKit;
using Models.Matchs.Enums;
using Models.Users.DTOs;
using Models.Users.Enums;
using Models.Users.Requests;

namespace Feature.Users.Services
{
    public class UserService
        : BaseService, IUserService, ICrudWithPagingService<User, CreateUserRequest, UpdateUserRequest, UserListItemDto, UserDetailDto, SearchUserRequest>
    {
        private readonly IStorageService _storageService;
        private readonly IRankService _rankService;

        public UserService(
            IUnitOfWork uow, 
            IStorageService storageService, 
            IRankService rankService) : base(uow)
        {
            _storageService = storageService;
            _rankService = rankService;
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var userRepo = _uow.Repository<User>();

            // Get profile
            var user = await userRepo.GetFirstAsync(
                predicate: u => u.Id == userId,
                selector: user => new
                {
                    user.Id,
                    user.Name,
                    user.AvatarUrl,
                    user.Level,
                    user.Exp
                }
            ) ?? throw new NotFoundException("User not found");

            // Get rank info
            var rankInfo = await _rankService.GetRankInfoAsync(userId);

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                AvatarUrl = user.AvatarUrl,
                Level = user.Level,
                Rank = rankInfo.Rank,
                RankScore = rankInfo.RankScore,
                WinningStreak = rankInfo.WinningStreak,
                WinningRate = rankInfo.WinningRate,
                Exp = user.Exp,
                ExpToUpLevel = LevelUtil.getLimit(user.Level),
                NumberOfMatchs = rankInfo.NumberOfMatchs,
            };
        }

        public async Task<ChangedResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request)
        {
            var userRepository = _uow.Repository<User>();
            var user = await GetEntityAsync<User>(userId);

            if (user.Name != request.Name 
                && await userRepository.ExistsAsync(u => u.Id != userId && u.Name == request.Name))
            {
                throw new BadRequestException("Name already used");
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

            return ChangedResponse.FromEntity(user);
        }

        public async Task<IEnumerable<UserListItemDto>> GetAllAsync()
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

        public async Task<Paginated<UserListItemDto>> GetPagingAsync(SearchUserRequest request)
        {
            var filter = PredicateBuilder.New<User>(true);
            if (request.Name != null)
            {
                filter = filter.And(e => e.Name.ToLower().Contains(request.Name.ToLower()));
            }
            if (request.Phone != null)
            {
                filter = filter.And(e => e.PhoneNumber.Contains(request.Phone));
            }
            if (request.Email != null)
            {
                filter = filter.And(e => e.Email.Contains(request.Email));
            }

            var entities = await _uow.Repository<User>().GetPagingAsync(
                predicate: filter,
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: request.OrderBy ?? nameof(User.Name),
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

            return entities;
        }

        

        private async Task ConfirmValidCreateDataAsync(CreateUserRequest request)
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
                    throw new BadRequestException("Email is already in use");
                }

                if (existedUser.PhoneNumber == request.PhoneNumber)
                {
                    throw new BadRequestException("Phone number is already in use");
                }

                if (existedUser.Name == request.Name)
                {
                    throw new BadRequestException("Display name is already in use");
                }
            }
        }

        private async Task ConfirmValidUpdateDataAsync(User entity, UpdateUserRequest request)
        {
            var userRepository = _uow.Repository<User>();

            if (entity.Name != request.Name 
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.Name == request.Name))
            {
                throw new BadRequestException("Display name is already in use");
            }

            if (entity.Email != request.Email
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.Email == request.Email))
            {
                throw new BadRequestException("Email is already in use");
            }

            if (entity.PhoneNumber != request.PhoneNumber
                && await userRepository.ExistsAsync(u => u.Id != entity.Id && u.PhoneNumber == request.PhoneNumber))
            {
                throw new BadRequestException("Phone number is already in use");
            }

            var role = await _uow.Repository<Role>().GetFirstAsync(
                predicate: r => r.Id == request.RoleId)
                ?? throw new BadRequestException("Role not found");
            if (role.Name == RoleName.SuperAdmin.ToString())
            {
                throw new ForbiddenException("Cannot change role of SuperAdmin");
            }
        }

        public async Task<UserDetailDto?> GetByIdAsync(int id)
        {
            return await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == id,
                selector: entity => new UserDetailDto
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
                    CreatedAt = entity.CreatedAt.ToLocalTime(),
                    RoleId = entity.RoleId,
                    RoleName = entity.Role.Name
                })
                ?? throw new NotFoundException("User not found");
        }

        public async Task<ChangedResponse> CreateAsync(CreateUserRequest request)
        {
            await ConfirmValidCreateDataAsync(request);

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

            await _uow.Repository<User>().AddAsync(user);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(user);
        }

        public async Task<ChangedResponse> UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == id,
                includes: e => e.Role)
                ?? throw new Exception("User not found");

            if (user.Role.Name == nameof(RoleName.SuperAdmin))
            {
                throw new ForbiddenException("Cannot update SuperAdmin"); 
            }

            await ConfirmValidUpdateDataAsync(user, request);

            user.Name = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Status = request.Status;
            user.Level = request.Level;
            user.Rank = request.Rank;
            user.RankScore = request.RankScore;
            user.Exp = request.Exp;
            user.RoleId = request.RoleId;

            if (request.Avatar != null)
            {
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    await _storageService.DeleteAsync(user.AvatarUrl);
                }

                user.AvatarUrl = await _storageService.SaveAsync(request.Avatar);
            }

            await _uow.Repository<User>().UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(user);
        }

        public async Task<ChangedResponse> DeleteAsync(int id)
        {
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: e => e.Id == id,
                includes: e => e.Role)
                ?? throw new Exception("User not found");

            if (user.Role.Name == nameof(RoleName.SuperAdmin))
            {
                throw new ForbiddenException("Cannot delete SuperAdmin");
            }

            await _uow.Repository<User>().DeleteAsync(user);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(user);
        }
    }
}
