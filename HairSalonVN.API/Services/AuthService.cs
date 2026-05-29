using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HairSalonVN.API.Helpers;
using HairSalonVN.API.Services.DTOs.Auth;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database.Constants;
using HairSalonVN.Database.Entities;

namespace HairSalonVN.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<RefreshToken> _rtRepo;
        private readonly JwtHelper _jwt;
        private readonly IConfiguration _cfg;

        public AuthService(
            IRepository<User> userRepo,
            IRepository<RefreshToken> rtRepo,
            JwtHelper jwt,
            IConfiguration cfg)
        {
            _userRepo = userRepo;
            _rtRepo = rtRepo;
            _jwt = jwt;
            _cfg = cfg;
        }

        public async Task<ApiResponse<TokenDto>> RegisterAsync(RegisterDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            var exists = await _userRepo.AnyAsync(u => u.Email == normalizedEmail);
            if (exists)
                return ApiResponse<TokenDto>.Fail("Email đã được sử dụng");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName.Trim(),
                Email = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone?.Trim(),
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return await IssueTokensAsync(user, "Đăng ký thành công");
        }

        public async Task<ApiResponse<TokenDto>> LoginAsync(LoginDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            var users = await _userRepo.FindAsync(
                u => u.Email == normalizedEmail && u.IsActive);
            var user = users.FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return ApiResponse<TokenDto>.Fail("Email hoặc mật khẩu không đúng");

            return await IssueTokensAsync(user, "Đăng nhập thành công");
        }

        public async Task<ApiResponse<TokenDto>> RefreshTokenAsync(string token)
        {
            var tokens = await _rtRepo.FindAsync(r => r.Token == token && !r.IsRevoked);
            var stored = tokens.FirstOrDefault();

            if (stored == null || stored.ExpiresAt < DateTime.UtcNow)
                return ApiResponse<TokenDto>.Fail("Refresh token không hợp lệ hoặc đã hết hạn");

            stored.IsRevoked = true;
            _rtRepo.Update(stored);

            var user = await _userRepo.GetByIdAsync(stored.UserId);
            if (user == null)
                return ApiResponse<TokenDto>.Fail("Tài khoản không tồn tại");

            return await IssueTokensAsync(user, "Token đã được làm mới");
        }

        public async Task RevokeTokenAsync(string token)
        {
            var tokens = await _rtRepo.FindAsync(r => r.Token == token);
            var stored = tokens.FirstOrDefault();
            if (stored == null) return;
            stored.IsRevoked = true;
            _rtRepo.Update(stored);
            await _rtRepo.SaveChangesAsync();
        }

        public Task<User?> GetUserByIdAsync(Guid id) => _userRepo.GetByIdAsync(id);

        private async Task<ApiResponse<TokenDto>> IssueTokensAsync(User user, string msg)
        {
            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            var refreshDays = GetConfiguredPositiveInt("RefreshTokenExpiryDays");
            var accessMinutes = GetConfiguredPositiveInt("AccessTokenExpiryMinutes");

            var rt = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
                CreatedAt = DateTime.UtcNow
            };

            await _rtRepo.AddAsync(rt);
            await _rtRepo.SaveChangesAsync();

            return ApiResponse<TokenDto>.Ok(new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                FullName = user.FullName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessMinutes),
                UserId = user.Id
            }, msg);
        }

        private int GetConfiguredPositiveInt(string key)
        {
            var value = _cfg[$"JwtSettings:{key}"];
            if (!int.TryParse(value, out var parsed) || parsed <= 0)
                throw new InvalidOperationException($"JwtSettings:{key} must be a positive integer.");

            return parsed;
        }
    }
}
