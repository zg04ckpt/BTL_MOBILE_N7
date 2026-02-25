using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Feature.Users.Entities;
using Feature.Users.Enums;
using Feature.Users.Interfaces;
using Feature.Users.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Feature.Users.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<LoginSesssionDto> LogInAsync(LoginRequest request)
        {
            var userRepository = _uow.Repository<User>();

            var user = await userRepository.GetFirstAsync(
                predicate: u => u.Email == request.Email,
                includes: u => u.Role
            );

            if (user == null)
            {
                throw new UnauthorizedException("Email hoặc mật khẩu không chính xác");
            }

            if (user.Status == AccountStatus.Banned)
            {
                throw new UnauthorizedException("Tài khoản đã bị khóa");
            }

            if (user.Status == AccountStatus.Deleted)
            {
                throw new UnauthorizedException("Tài khoản không tồn tại");
            }

            if (user.Status == AccountStatus.Inactive)
            {
                throw new UnauthorizedException("Tài khoản chưa được kích hoạt");
            }

            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Email hoặc mật khẩu không chính xác");
            }

            var token = GenerateJwtToken(user);

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
                throw new BadRequestException("Email đã được sử dụng");
            }

            var existingUserByPhone = await userRepository.ExistsAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (existingUserByPhone)
            {
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
        }

        private static string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvUtil.GetEnv(EnvUtil.Keys.QUIZBATTLE_SECRET_KEY)));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
