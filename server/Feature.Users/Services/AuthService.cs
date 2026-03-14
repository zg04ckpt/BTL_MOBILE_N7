using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Users.DTOs;
using Models.Users.Enums;
using Models.Users.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Feature.Users.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, IConfiguration configuration) : base(uow)
        {
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
                throw new UnauthorizedException("Invalid email or password");
            }

            if (user.Status == AccountStatus.Banned)
            {
                throw new UnauthorizedException("Account is banned");
            }

            if (user.Status == AccountStatus.Deleted)
            {
                throw new UnauthorizedException("Account does not exist");
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new UnauthorizedException("Account is not activated");
            }

            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid email or password");
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
                throw new BadRequestException("Email is already in use");
            }

            var existingUserByPhone = await userRepository.ExistsAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingUserByPhone)
            {
                throw new BadRequestException("Phone number is already in use");
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
