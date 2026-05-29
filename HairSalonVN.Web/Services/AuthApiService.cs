using HairSalonVN.Web.Models.Auth;
using HairSalonVN.Web.Models.Shared;

namespace HairSalonVN.Web.Services
{
    public class AuthApiService : ApiClientBase
    {
        public AuthApiService(HttpClient http, IHttpContextAccessor ctx)
            : base(http, ctx) { }

        public Task<ApiResponse<TokenDto>?> RegisterAsync(RegisterViewModel vm)
            => PostAsync<TokenDto>("auth/register", new
            {
                vm.FullName,
                vm.Email,
                vm.Password,
                vm.ConfirmPassword,
                vm.Phone
            });

        public Task<ApiResponse<TokenDto>?> LoginAsync(
            string email, string password)
            => PostAsync<TokenDto>("auth/login",
                new { Email = email, Password = password });

        public Task<ApiResponse<TokenDto>?> RefreshAsync(string refreshToken)
            => PostAsync<TokenDto>("auth/refresh",
                new { RefreshToken = refreshToken });

        public Task<ApiResponse<object>?> LogoutAsync(string refreshToken)
            => PostAsync<object>("auth/logout",
                new { RefreshToken = refreshToken });

        public Task<ApiResponse<ProfileDto>?> GetMeAsync()
        {
            var userIdStr = _ctx.HttpContext?.Session.GetString("UserId");
            if (Guid.TryParse(userIdStr, out var userId))
                return GetAsync<ProfileDto>($"auth/me?userId={userId}");
            return Task.FromResult<ApiResponse<ProfileDto>?>(null);
        }

        public Task<ApiResponse<ProfileDto>?> GetUserByIdAsync(Guid userId)
            => GetAsync<ProfileDto>($"auth/me?userId={userId}");

        public Task<ApiResponse<ProfileDto>?> UpdateProfileAsync(Guid userId, string fullName, string? phone)
            => PutAsync<ProfileDto>($"profile/{userId}", new
            {
                userId,
                fullName,
                phone
            });
    }

    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    // TokenDto (mirror) dùng để deserialize
    public class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public Guid UserId { get; set; }
    }

}
