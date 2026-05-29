using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Appointment;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _svc;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentService svc, ILogger<AppointmentsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        private IActionResult ValidationError()
            => BadRequest(ApiResponse<object>.Fail(
                ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList()));

        /// <summary>GET /api/appointments</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _svc.GetAllAsync(Guid.Empty, "Admin");
            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/appointments/my</summary>
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var result = await _svc.GetMyAppointmentsAsync(Guid.Empty, "Admin");
            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/appointments/{id}</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _svc.GetByIdAsync(id, Guid.Empty, "Admin");
            return result == null
                ? NotFound(ApiResponse<object>.Fail("Khong tim thay lich hen"))
                : Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>POST /api/appointments</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationError();
            var result = await _svc.CreateAsync(dto, Guid.Empty);
            return result.Success
                ? Created($"/api/appointments/{result.Data!.Id}", result)
                : BadRequest(result);
        }

        /// <summary>PUT /api/appointments/{id}/status</summary>
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            if (!dto.IsValid())
                return BadRequest(ApiResponse<object>.Fail("Trang thai khong hop le"));
            var result = await _svc.UpdateStatusAsync(id, dto.Status!, Guid.Empty, "Admin");
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>GET /api/appointments/available-slots</summary>
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetSlots(
            [FromQuery] Guid staffId,
            [FromQuery] Guid serviceId,
            [FromQuery] DateTime date)
        {
            try
            {
                var slots = await _svc.GetAvailableSlotsAsync(staffId, serviceId, date.Date);
                return Ok(ApiResponse<object>.Ok(slots));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading slots");
                return StatusCode(500, ApiResponse<object>.Fail("Loi tai khung gio."));
            }
        }

        /// <summary>POST /api/appointments/guest</summary>
        [HttpPost("guest")]
        public async Task<IActionResult> GuestCreate([FromBody] AppointmentCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationError();
            if (!dto.IsGuest)
                return BadRequest(ApiResponse<object>.Fail("Yeu cau khong hop le"));
            var result = await _svc.GuestCreateAsync(dto);
            return result.Success
                ? Created($"/api/appointments/{result.Data!.Id}", result)
                : BadRequest(result);
        }

        /// <summary>GET /api/appointments/slots</summary>
        [HttpGet("slots")]
        public async Task<IActionResult> GetSlotsByDate(
            [FromQuery] Guid staffId,
            [FromQuery] Guid serviceId,
            [FromQuery] string date)
        {
            if (staffId == Guid.Empty || serviceId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("staffId and serviceId are required"));

            if (string.IsNullOrWhiteSpace(date))
                return BadRequest(ApiResponse<object>.Fail("date is required (yyyy-MM-dd)"));

            DateTime parsed;
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                if (!DateTime.TryParse(date, out parsed))
                    return BadRequest(ApiResponse<object>.Fail("Invalid date format"));

            try
            {
                var slots = await _svc.GetAvailableSlotsAsync(staffId, serviceId, parsed.Date);
                return Ok(ApiResponse<object>.Ok(slots));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading slots");
                return StatusCode(500, ApiResponse<object>.Fail("Loi tai khung gio."));
            }
        }
    }
}
