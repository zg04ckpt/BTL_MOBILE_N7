using CNLib.Services.Logs;
using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Enums;
using Feature.Users.Interfaces;
using Feature.Users.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Feature.Users.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly ILogService<AuthService> _logService;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, ILogService<AuthService> logService, IConfiguration configuration) : base(uow)
        {
            _logService = logService;
            _configuration = configuration;
        }

        public async Task<LoginSesssionDto> LogInAsync(LoginRequest request, int? loginLiveTimeMinutes = null)
        {
            var userRepository = _uow.Repository<User>();

            var user = await userRepository.GetFirstAsync(
                predicate: u => u.Email == request.Email,
                includes: u => u.Role
            );

            if (user == null)
            {
                _logService.LogError($"Login failed: {request.Email}");
                throw new UnauthorizedException("Email hoặc mật khẩu không chính xác");
            }

            if (user.Status == AccountStatus.Banned)
            {
                _logService.LogError($"Login blocked: Banned account #{user.Id}");
                throw new UnauthorizedException("Tài khoản đã bị khóa");
            }

            if (user.Status == AccountStatus.Deleted)
            {
                _logService.LogError($"Login blocked: Deleted account #{user.Id}");
                throw new UnauthorizedException("Tài khoản không tồn tại");
            }

            if (user.Status == AccountStatus.Inactive)
            {
                _logService.LogError($"Login blocked: Inactive account #{user.Id}");
                throw new UnauthorizedException("Tài khoản chưa được kích hoạt");
            }

            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logService.LogError($"Login failed: Wrong password for #{user.Id}");
                throw new UnauthorizedException("Email hoặc mật khẩu không chính xác");
            }

            var token = GenerateJwtToken(user, loginLiveTimeMinutes ?? 10080);

            return new LoginSesssionDto
            {
                Id = user.Id,
                AvatarUrl = user.AvatarUrl,
                Level = user.Level,
                Name = user.Name,
                Rank = user.Rank,
                RankScore = user.RankScore,
                AccessToken = token
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var userRepository = _uow.Repository<User>();

            var existingUserByEmail = await userRepository.ExistsAsync(u => u.Email == request.Email);
            if (existingUserByEmail)
            {
                _logService.LogError($"Register failed: Email exists - {request.Email}");
                throw new BadRequestException("Email đã được sử dụng");
            }

            var existingUserByPhone = await userRepository.ExistsAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingUserByPhone)
            {
                _logService.LogError($"Register failed: Phone exists - {request.PhoneNumber}");
                throw new BadRequestException("Số điện thoại đã được sử dụng");
            }

            var newUser = new User
            {
                Name = request.DisplayName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Status = AccountStatus.Active,
                Level = 1,
                Rank = 0,
                RankScore = 0,
                Exp = 0,
                CreatedAt = DateTime.UtcNow,
                RoleId = 2,
                AvatarUrl = string.Empty
            };

            await userRepository.AddAsync(newUser);
            await _uow.SaveChangesAsync();
            
            _logService.LogSuccess($"User registered: {newUser.Email}");
        }

        private string GenerateJwtToken(User user, int loginLiveTimeMinutes)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var issuer = _configuration["JWT:Issuer"] ?? "QuizBattle.API";
            var audience = _configuration["JWT:Audience"] ?? "QuizBattle.Client";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_SECRET_KEY)));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(loginLiveTimeMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
