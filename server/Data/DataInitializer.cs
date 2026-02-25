using Core.Interfaces;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class DataInitializer
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(AppDbContext context, IUnitOfWork unitOfWork, ILogger<DataInitializer> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task CheckAndUpdateFromMigrationAsync()
        {
            try
            {
                _logger.LogInformation("Start migrating...");
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while migrating the database: " + ex.Message);
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

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError("An error occurred while initializing the database: " + ex.Message);
                throw;
            }
        }
    }
}
