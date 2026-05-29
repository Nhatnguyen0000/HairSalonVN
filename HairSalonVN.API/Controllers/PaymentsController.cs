using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly SalonDbContext _ctx;

        public PaymentsController(SalonDbContext ctx) => _ctx = ctx;

        /// <summary>GET /api/payments - List all payments</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _ctx.Payments
                .AsNoTracking()
                .Include(p => p.Appointment)
                    .ThenInclude(a => a!.Service)
                .Include(p => p.Appointment)
                    .ThenInclude(a => a!.Customer)
                .OrderByDescending(p => p.PaidAt)
                .ToListAsync();

            var result = payments.Select(p =>
            {
                var appt = p.Appointment;
                string customerName;
                if (appt != null && appt.IsGuestBooking)
                    customerName = appt.GuestName ?? "Khach";
                else if (appt != null && appt.Customer != null)
                    customerName = appt.Customer.FullName;
                else
                    customerName = "Khach";

                return new
                {
                    p.Id,
                    p.AppointmentId,
                    BookingCode = appt?.BookingCode ?? "",
                    CustomerName = customerName,
                    ServiceName = appt?.Service?.Name ?? "",
                    p.Amount,
                    p.Method,
                    p.Status,
                    p.PaidAt
                };
            }).ToList();

            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>GET /api/payments/summary - Payment summary</summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var monthStart = new DateTime(System.DateTime.UtcNow.Year, System.DateTime.UtcNow.Month, 1);
            var paid = await _ctx.Payments
                .AsNoTracking()
                .Where(p => p.Status == "Paid" && p.PaidAt >= monthStart)
                .ToListAsync();

            var totalAll = await _ctx.Payments
                .AsNoTracking()
                .Where(p => p.Status == "Paid")
                .SumAsync(p => p.Amount);

            return Ok(ApiResponse<object>.Ok(new
            {
                TotalThisMonth = paid.Sum(p => p.Amount),
                CountThisMonth = paid.Count,
                TotalAll = totalAll
            }));
        }
    }
}
