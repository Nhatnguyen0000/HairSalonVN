using System;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Profile;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IRepository<Database.Entities.User> _userRepo;
        private readonly IAuthService _authService;

        public ProfileController(
            IRepository<Database.Entities.User> userRepo,
            IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        /// <summary>GET /api/profile/{userId} - Get user profile by ID</summary>
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            
            if (user == null)
                return NotFound(ApiResponse<object>.Fail("Không tìm thấy người dùng"));

            var profile = new ProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(ApiResponse<object>.Ok(profile));
        }

        /// <summary>GET /api/profile/me?userId={guid} - Get current user profile (simple auth)</summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentProfile([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("UserId không hợp lệ"));

            var user = await _userRepo.GetByIdAsync(userId);
            
            if (user == null)
                return NotFound(ApiResponse<object>.Fail("Không tìm thấy người dùng"));

            var profile = new ProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(ApiResponse<object>.Ok(profile));
        }

        /// <summary>PUT /api/profile/{userId} - Update user profile</summary>
        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> Update(Guid userId, [FromBody] ProfileUpdateDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ"));

            // Validate userId matches
            if (dto.UserId != userId)
                return BadRequest(ApiResponse<object>.Fail("UserId không khớp"));

            if (dto.UserId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("UserId không hợp lệ"));

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return NotFound(ApiResponse<object>.Fail("Không tìm thấy người dùng"));

            // Validate input
            var errors = ValidateUpdate(dto);
            if (errors.Any())
                return BadRequest(ApiResponse<object>.Fail(errors));

            // Update fields
            user.FullName = dto.FullName.Trim();
            user.Phone = dto.Phone?.Trim();

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

            var profile = new ProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(ApiResponse<object>.Ok(profile, "Cập nhật hồ sơ thành công"));
        }

        private static System.Collections.Generic.List<string> ValidateUpdate(ProfileUpdateDto dto)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (string.IsNullOrWhiteSpace(dto.FullName))
                errors.Add("Họ tên không được để trống");
            else if (dto.FullName.Trim().Length < 2)
                errors.Add("Họ tên phải có ít nhất 2 ký tự");
            else if (dto.FullName.Trim().Length > 100)
                errors.Add("Họ tên không được quá 100 ký tự");

            // Phone is optional but if provided, validate format
            if (!string.IsNullOrWhiteSpace(dto.Phone) && !IsValidPhone(dto.Phone))
                errors.Add("Số điện thoại không hợp lệ (VD: 0912345678, +84912345678)");

            return errors;
        }

        private static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return true;
            var cleaned = phone.Trim().Replace(" ", "").Replace("-", "");
            if (System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^(\+84)?[0-9]{9,10}$"))
                return true;
            return false;
        }
    }
}
