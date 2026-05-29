using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Services;
using HairSalonVN.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _svc;

        public ServicesController(IServiceService svc) => _svc = svc;

        private IActionResult ValidationError()
            => BadRequest(ApiResponse<object>.Fail(
                ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList()));

        /// <summary>GET /api/services - List all active services</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _svc.GetAllActiveAsync();
            var result = services.Select(s => new
            {
                s.Id,
                s.Name,
                s.Description,
                s.ImageUrl,
                s.Price,
                s.DurationMinutes,
                s.IsActive
            });
            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/services/by-service/{serviceId} - Staff by service</summary>
        [HttpGet("by-service/{serviceId:guid}")]
        public async Task<IActionResult> GetByServiceStaff(System.Guid serviceId)
        {
            var r = await _svc.GetByServiceStaffAsync(serviceId);
            return Ok(ApiResponse<object>.Ok(r));
        }

        /// <summary>GET /api/services/{id}</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(System.Guid id)
        {
            var r = await _svc.GetByIdAsync(id);
            return r == null
                ? NotFound(ApiResponse<object>.Fail("Khong tim thay dich vu"))
                : Ok(ApiResponse<object>.Ok(r));
        }

        /// <summary>POST /api/services - Create service</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationError();
            var r = await _svc.CreateAsync(dto);
            return Created($"/api/services/{r.Id}", ApiResponse<object>.Ok(r));
        }

        /// <summary>PUT /api/services/{id}</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(System.Guid id, [FromBody] ServiceUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationError();
            var r = await _svc.UpdateAsync(id, dto);
            return r.Success ? Ok(r) : BadRequest(r);
        }

        /// <summary>DELETE /api/services/{id}</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(System.Guid id)
        {
            await _svc.SoftDeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null, "Da xoa dich vu"));
        }
    }
}
