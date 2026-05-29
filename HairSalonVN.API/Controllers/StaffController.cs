using System;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Staff;
using HairSalonVN.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _svc;

        public StaffController(IStaffService svc) => _svc = svc;

        private static object MapStaff(Database.Entities.Staff s) => new
        {
            StaffId = s.Id,
            StaffName = s.User?.FullName ?? "",
            s.Id,
            s.Specialty,
            s.Bio,
            AvatarUrl = s.AvatarUrl ?? "",
            IsAvailable = s.IsAvailable,
            UserName = s.User?.FullName ?? "",
            UserEmail = s.User?.Email ?? "",
            UserPhone = s.User?.Phone ?? "",
            Services = s.StaffServices?.Where(ss => ss.Service != null)
                .Select(ss => ss.Service!.Name).Distinct().ToList() ?? new(),
            WorkingHours = s.WorkingHours?.Select(w => new
            {
                w.DayOfWeek,
                StartTime = $"{(int)w.StartTime.TotalHours:D2}:{w.StartTime.Minutes:D2}",
                EndTime = $"{(int)w.EndTime.TotalHours:D2}:{w.EndTime.Minutes:D2}"
            }).ToList() ?? new()
        };

        /// <summary>GET /api/staff</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? serviceId = null)
        {
            var list = serviceId.HasValue
                ? await _svc.GetByServiceIdAsync(serviceId.Value)
                : await _svc.GetAllAsync();

            return Ok(ApiResponse<object>.Ok(list.Select(MapStaff)));
        }

        /// <summary>GET /api/staff/me</summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var staff = await _svc.GetByUserIdAsync(Guid.Empty);
            if (staff == null)
                return NotFound(ApiResponse<object>.Fail("Khong tim thay ho so stylist"));

            return Ok(ApiResponse<object>.Ok(MapStaff(staff)));
        }

        /// <summary>GET /api/staff/{id}</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var r = await _svc.GetByIdAsync(id);
            if (r == null)
                return NotFound(ApiResponse<object>.Fail("Khong tim thay nhan vien"));

            return Ok(ApiResponse<object>.Ok(MapStaff(r)));
        }

        /// <summary>PUT /api/staff/{id}/working-hours</summary>
        [HttpPut("{id:guid}/working-hours")]
        public async Task<IActionResult> UpdateWorkingHours(Guid id, [FromBody] System.Collections.Generic.List<WorkingHourUpdateDto>? dto)
        {
            if (dto == null || dto.Any(w => w.DayOfWeek < 0 || w.DayOfWeek > 6 || w.StartTime >= w.EndTime))
                return BadRequest(ApiResponse<object>.Fail("Gio lam viec khong hop le"));

            var r = await _svc.UpdateWorkingHoursAsync(id, dto);
            return r.Success ? Ok(r) : BadRequest(r);
        }
    }
}
