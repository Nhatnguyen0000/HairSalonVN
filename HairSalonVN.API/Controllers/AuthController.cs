using System;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Auth;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        /// <summary>POST /api/auth/register</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Validate null
            if (dto == null)
                return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ"));

            // Validate required fields
            var errors = ValidateRegister(dto);
            if (errors.Any())
                return BadRequest(ApiResponse<object>.Fail(errors));

            var result = await _auth.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>POST /api/auth/login</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ"));

            // Validate required fields
            var errors = ValidateLogin(dto);
            if (errors.Any())
                return BadRequest(ApiResponse<object>.Fail(errors));

            var result = await _auth.LoginAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>POST /api/auth/refresh</summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest(ApiResponse<object>.Fail("Refresh token không hợp lệ"));

            var result = await _auth.RefreshTokenAsync(dto.RefreshToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>POST /api/auth/logout</summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest(ApiResponse<object>.Fail("Refresh token không hợp lệ"));

            await _auth.RevokeTokenAsync(dto.RefreshToken);
            return Ok(ApiResponse<object>.Ok(null, "Đăng xuất thành công"));
        }

        /// <summary>GET /api/auth/me?userId={guid} - Get current user profile</summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("UserId không hợp lệ"));

            var user = await _auth.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(ApiResponse<object>.Fail("Không tìm thấy người dùng"));

            return Ok(ApiResponse<object>.Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Role,
                user.Phone
            }));
        }

        /// <summary>GET /api/auth/test - Test endpoint</summary>
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(ApiResponse<object>.Ok(new
            {
                message = "Auth API hoạt động!",
                timestamp = DateTime.UtcNow
            }));
        }

        /// <summary>POST /api/auth/test - Test POST endpoint</summary>
        [HttpPost("test")]
        public IActionResult TestPost([FromBody] object? data)
        {
            return Ok(ApiResponse<object>.Ok(new
            {
                received = data,
                message = "Auth POST API hoạt động!",
                timestamp = DateTime.UtcNow
            }));
        }

        // Validation helpers
        private static System.Collections.Generic.List<string> ValidateRegister(RegisterDto dto)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (string.IsNullOrWhiteSpace(dto.FullName))
                errors.Add("Họ tên không được để trống");
            else if (dto.FullName.Trim().Length < 2)
                errors.Add("Họ tên phải có ít nhất 2 ký tự");
            else if (dto.FullName.Trim().Length > 100)
                errors.Add("Họ tên không được quá 100 ký tự");

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email không được để trống");
            else if (!IsValidEmail(dto.Email))
                errors.Add("Định dạng email không hợp lệ");

            if (string.IsNullOrWhiteSpace(dto.Password))
                errors.Add("Mật khẩu không được để trống");
            else if (dto.Password.Length < 6)
                errors.Add("Mật khẩu phải có ít nhất 6 ký tự");

            if (!string.IsNullOrWhiteSpace(dto.ConfirmPassword) && dto.Password != dto.ConfirmPassword)
                errors.Add("Mật khẩu xác nhận không khớp");

            // Phone is optional but if provided, validate format
            if (!string.IsNullOrWhiteSpace(dto.Phone) && !IsValidPhone(dto.Phone))
                errors.Add("Số điện thoại không hợp lệ (VD: 0912345678, +84912345678)");

            return errors;
        }

        private static System.Collections.Generic.List<string> ValidateLogin(LoginDto dto)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email không được để trống");
            else if (!IsValidEmail(dto.Email))
                errors.Add("Định dạng email không hợp lệ");

            if (string.IsNullOrWhiteSpace(dto.Password))
                errors.Add("Mật khẩu không được để trống");

            return errors;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email.Trim();
            }
            catch { return false; }
        }

        private static bool IsValidPhone(string phone)
        {
            // Accept Vietnamese phone formats:
            // 0912345678 (10 digits starting with 0)
            // +84912345678 (11 digits with +84)
            // 912345678 (9 digits starting with 9)
            if (string.IsNullOrWhiteSpace(phone)) return true; // Optional field

            var cleaned = phone.Trim().Replace(" ", "").Replace("-", "");
            
            // Pattern: optional +84, then 9 or 10 digits
            if (System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^(\+84)?[0-9]{9,10}$"))
                return true;

            return false;
        }
    }
}
