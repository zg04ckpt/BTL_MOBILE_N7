using CNLib.Services.Logs;
using Core.Interfaces;
using Core.Utilities;
using Feature.Settings.Interfaces;
using Feature.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Models.Events.Entities;
using Models.Events.Enums;
using Models.Users.Enums;

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

                var eventRewardRepo = _unitOfWork.Repository<EventReward>();
                if (await eventRewardRepo.CountAsync() == 0)
                {
                    var defaultRewards = Enum.GetValues<EventRewardType>()
                        .Select(type => new EventReward
                        {
                            Name = type switch
                            {
                                EventRewardType.RankProtectionCard => "Thẻ bảo vệ rank",
                                EventRewardType.ExpScore => "Kinh nghiệm",
                                EventRewardType.Gold => "Vàng",
                                EventRewardType.MatchLoudspeaker => "Match Loudspeaker",
                                _ => type.ToString()
                            },
                            Type = type,
                            Desc = $"Default reward for {type}",
                            Unit = type switch
                            {
                                EventRewardType.RankProtectionCard => "card",
                                EventRewardType.ExpScore => "exp",
                                EventRewardType.Gold => "gold",
                                EventRewardType.MatchLoudspeaker => "item",
                                _ => "unit"
                            },
                            ClaimedRewards = new()
                        })
                        .ToArray();

                    await eventRewardRepo.AddAsync(defaultRewards);
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
