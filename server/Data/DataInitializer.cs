using Core.Interfaces;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Enums;
using Feature.Settings.Interfaces;
using Microsoft.EntityFrameworkCore;
using CNLib.Services.Logs;

namespace Data
{
    public class DataInitializer
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService<DataInitializer> _logger;
        private readonly ISystemConfigurationService _configurationService;

        public DataInitializer(
            AppDbContext context, 
            IUnitOfWork unitOfWork,
            ILogService<DataInitializer> logger,
            ISystemConfigurationService configurationService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _configurationService = configurationService;
        }

        public async Task CheckAndUpdateFromMigrationAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Migration failed: {ex.Message}");
                throw;
            }
        }

        public async Task CheckAndInitDefaultDataAsync()
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var roleRepo = _unitOfWork.Repository<Role>();
                if (await roleRepo.CountAsync() == 0)
                {
                    var roles = new Role[]
                    {
                        new Role
                        {
                            Id = 1,
                            Name = nameof(RoleName.SuperAdmin),
                        },
                        new Role
                        {
                            Id = 2,
                            Name = nameof(RoleName.Admin),
                        },
                        new Role
                        {
                            Id = 3,
                            Name = nameof(RoleName.Moderator),
                        },
                        new Role
                        {
                            Id = 4,
                            Name = nameof(RoleName.Editor),
                        },
                        new Role
                        {
                            Id = 5,
                            Name = nameof(RoleName.User),
                        },
                    };

                    await roleRepo.AddAsync(roles);
                }
                

                var userRepo = _unitOfWork.Repository<User>();
                if (await userRepo.CountAsync() == 0)
                {
                    var users = new User[]
                    {
                        new()
                        {
                            Name = "Super Admin",
                            Email = "superadmin@quizbattle.com",
                            AvatarUrl = "https://res.cloudinary.com/dvk5yt0oi/image/upload/v1758957778/zlearn/images/hhtcmdxecquxqnsor3ip.jpg",
                            PasswordHash = PasswordHasher.HashPassword(EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_ADMIN_PASS)),
                            CreatedAt = DateTime.UtcNow,
                            Exp = 0,
                            Level = 1,
                            PhoneNumber = "0000",
                            Rank = 0,
                            RankScore = 0,
                            RoleId = 1,
                            Status = AccountStatus.Active
                        },
                        new()
                        {
                            Name = "Admin",
                            Email = "admin@quizbattle.com",
                            AvatarUrl = "https://res.cloudinary.com/dvk5yt0oi/image/upload/v1758957778/zlearn/images/hhtcmdxecquxqnsor3ip.jpg",
                            PasswordHash = PasswordHasher.HashPassword(EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_ADMIN_PASS)),
                            CreatedAt = DateTime.UtcNow,
                            Exp = 0,
                            Level = 1,
                            PhoneNumber = "0000",
                            Rank = 0,
                            RankScore = 0,
                            RoleId = 2,
                            Status = AccountStatus.Active
                        }
                    };
                    await userRepo.AddAsync(users);
                }

                // Initialize default system configurations
                await _configurationService.InitializeDefaultConfigurationsAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError($"Data init failed: {ex.Message}");
                throw;
            }
        }
    }
}
