using System;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database;
using HairSalonVN.Database.Constants;
using HairSalonVN.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IRepository<Review> _reviewRepo;
        private readonly IRepository<Appointment> _apptRepo;
        private readonly SalonDbContext _ctx;

        public ReviewsController(
            IRepository<Review> reviewRepo,
            IRepository<Appointment> apptRepo,
            SalonDbContext ctx)
        {
            _reviewRepo = reviewRepo;
            _apptRepo = apptRepo;
            _ctx = ctx;
        }

        /// <summary>GET /api/reviews - List all reviews</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ctx.Reviews
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt,
                    CustomerName = r.Appointment!.Customer != null
                        ? r.Appointment.Customer.FullName : "Khach hang",
                    ServiceName = r.Appointment.Service != null
                        ? r.Appointment.Service.Name : "Dich vu",
                    StaffName = r.Appointment.Staff != null && r.Appointment.Staff.User != null
                        ? r.Appointment.Staff.User.FullName : "Stylist"
                })
                .ToListAsync();

            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/reviews/service/{serviceId}</summary>
        [HttpGet("service/{serviceId:guid}")]
        public async Task<IActionResult> GetByService(Guid serviceId)
        {
            var reviews = await _reviewRepo.FindAsync(
                r => r.Appointment != null && r.Appointment.ServiceId == serviceId,
                r => r.Appointment!,
                r => r.Appointment!.Customer!);

            var result = reviews.Select(r => new
            {
                r.Id,
                r.Rating,
                r.Comment,
                r.CreatedAt,
                CustomerName = r.Appointment?.Customer?.FullName ?? "Khach hang"
            });

            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/reviews/staff/{staffId}</summary>
        [HttpGet("staff/{staffId:guid}")]
        public async Task<IActionResult> GetByStaff(Guid staffId)
        {
            var reviews = await _reviewRepo.FindAsync(
                r => r.Appointment != null && r.Appointment.StaffId == staffId,
                r => r.Appointment!,
                r => r.Appointment!.Customer!);

            var result = reviews.Select(r => new
            {
                r.Id,
                r.Rating,
                r.Comment,
                r.CreatedAt,
                CustomerName = r.Appointment?.Customer?.FullName ?? "Khach hang",
                ServiceName = r.Appointment?.Service?.Name ?? "Dich vu"
            });

            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/reviews/appointment/{appointmentId}</summary>
        [HttpGet("appointment/{appointmentId:guid}")]
        public async Task<IActionResult> GetByAppointment(Guid appointmentId)
        {
            var appt = await _apptRepo.GetByIdAsync(appointmentId);
            if (appt == null)
                return NotFound(ApiResponse<object>.Fail("Khong tim thay lich hen"));

            var review = (await _reviewRepo.FindAsync(r => r.AppointmentId == appointmentId)).FirstOrDefault();

            if (review == null)
                return Ok(ApiResponse<object>.Ok(new { hasReview = false }));

            return Ok(ApiResponse<object>.Ok(new
            {
                hasReview = true,
                review.Id,
                review.Rating,
                review.Comment
            }));
        }

        /// <summary>POST /api/reviews - Create review</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            if (dto == null || dto.AppointmentId == Guid.Empty)
                return BadRequest(ApiResponse<object>.Fail("Du lieu danh gia khong hop le"));

            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest(ApiResponse<object>.Fail("Rating phai tu 1 den 5 sao"));

            var appt = await _apptRepo.GetByIdAsync(dto.AppointmentId);
            if (appt == null)
                return NotFound(ApiResponse<object>.Fail("Khong tim thay lich hen"));

            if (appt.Status != AppointmentStatus.Completed)
                return BadRequest(ApiResponse<object>.Fail("Chi co the danh gia khi lich hen da hoan thanh"));

            var existingReview = (await _reviewRepo.FindAsync(r => r.AppointmentId == dto.AppointmentId)).FirstOrDefault();
            if (existingReview != null)
                return BadRequest(ApiResponse<object>.Fail("Ban da danh gia lich hen nay roi"));

            var review = new Review
            {
                Id = Guid.NewGuid(),
                AppointmentId = dto.AppointmentId,
                CustomerId = appt.CustomerId ?? Guid.Empty,
                Rating = dto.Rating,
                Comment = dto.Comment?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            return Ok(ApiResponse<object>.Ok(new { review.Id, review.Rating }, "Cam on ban da danh gia!"));
        }
    }

    public class CreateReviewDto
    {
        public Guid AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
