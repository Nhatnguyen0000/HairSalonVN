using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Appointment;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Services;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database;
using HairSalonVN.Database.Constants;
using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HairSalonVN.API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _apptRepo;
        private readonly IRepository<Service> _svcRepo;
        private readonly IRepository<WorkingHour> _whRepo;
        private readonly IRepository<Staff> _staffRepo;
        private readonly SalonDbContext _ctx;
        private readonly EmailService _email;

        // Chỉ lock cho việc booking concurrency, không cache data
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public AppointmentService(
            IRepository<Appointment> a,
            IRepository<Service> s,
            IRepository<WorkingHour> w,
            IRepository<Staff> staffRepo,
            SalonDbContext ctx,
            EmailService email)
        {
            _apptRepo = a;
            _svcRepo = s;
            _whRepo = w;
            _staffRepo = staffRepo;
            _ctx = ctx;
            _email = email;
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync(Guid requesterId, string role)
        {
            // Luon doc tu DB, khong cache
            var list = await _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Include(a => a.Staff).ThenInclude(s => s!.User)
                .Include(a => a.Review)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetMyAppointmentsAsync(Guid requesterId, string role)
        {
            var query = _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Include(a => a.Staff).ThenInclude(s => s!.User)
                .Include(a => a.Review)
                .AsQueryable();

            if (role == Roles.Customer)
                query = query.Where(a => a.CustomerId == requesterId);
            else if (role == Roles.Staff)
            {
                var staff = await _staffRepo.FindAsync(s => s.UserId == requesterId);
                var staffId = staff.FirstOrDefault()?.Id;
                if (staffId != null)
                    query = query.Where(a => a.StaffId == staffId);
            }

            var list = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByStaffIdAsync(Guid staffId)
        {
            var list = await _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Include(a => a.Staff).ThenInclude(s => s!.User)
                .Include(a => a.Review)
                .Where(a => a.StaffId == staffId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetMyShiftAppointmentsAsync(Guid userId)
        {
            var staff = await _staffRepo.FindAsync(s => s.UserId == userId);
            var staffId = staff.FirstOrDefault()?.Id;
            if (staffId == null)
                return Enumerable.Empty<AppointmentResponseDto>();

            var list = await _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Include(a => a.Staff).ThenInclude(s => s!.User)
                .Include(a => a.Review)
                .Where(a => a.StaffId == staffId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetTodayAppointmentsAsync(Guid userId)
        {
            var staff = await _staffRepo.FindAsync(s => s.UserId == userId);
            var staffId = staff.FirstOrDefault()?.Id;
            if (staffId == null)
                return Enumerable.Empty<AppointmentResponseDto>();

            var today = DateTime.Today;
            var list = await _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Include(a => a.Staff).ThenInclude(s => s!.User)
                .Include(a => a.Review)
                .Where(a => a.StaffId == staffId && a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
            return list.Select(MapToDto);
        }

        public async Task<AppointmentResponseDto?> GetByIdAsync(Guid id, Guid requesterId, string role)
        {
            var appt = await _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Include(a => a.Staff).ThenInclude(s => s!.User)
                .Include(a => a.Review)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appt == null) return null;
            if (role == Roles.Customer && appt.CustomerId != requesterId) return null;
            if (role == Roles.Staff)
            {
                var staff = await _staffRepo.FindAsync(s => s.UserId == requesterId);
                if (staff.FirstOrDefault()?.Id != appt.StaffId) return null;
            }
            return MapToDto(appt);
        }

        public async Task<ApiResponse<AppointmentResponseDto>> CreateAsync(AppointmentCreateDto dto, Guid customerId)
        {
            var service = await _svcRepo.GetByIdAsync(dto.ServiceId);
            if (service == null || !service.IsActive)
                return ApiResponse<AppointmentResponseDto>.Fail("Dich vu khong ton tai hoac da ngung");

            var staff = await _staffRepo.GetByIdAsync(dto.StaffId);
            if (staff == null || !staff.IsAvailable)
                return ApiResponse<AppointmentResponseDto>.Fail("Stylist khong ton tai hoac da ngung nhan lich");

            var startTime = dto.AppointmentDate;
            if (startTime <= DateTime.Now)
                return ApiResponse<AppointmentResponseDto>.Fail("Vui long chon thoi gian trong tuong lai");

            var endTime = startTime.AddMinutes(service.DurationMinutes);

            var slotLock = GetStaffDateLock(dto.StaffId, startTime.Date);
            await slotLock.WaitAsync();
            try
            {
                if (await HasSlotConflictAsync(dto.StaffId, startTime, endTime))
                    return ApiResponse<AppointmentResponseDto>.Fail("Khung gio nay da co nguoi dat");

                var appt = new Appointment
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    StaffId = dto.StaffId,
                    ServiceId = dto.ServiceId,
                    AppointmentDate = startTime,
                    Status = AppointmentStatus.Pending,
                    BookingCode = GenerateBookingCode(),
                    Notes = dto.Notes?.Trim(),
                    IsGuestBooking = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _apptRepo.AddAsync(appt);
                await _apptRepo.SaveChangesAsync();

                _ = _email.SendBookingConfirmationAsync(appt.Id);

                // Load lai data moi nhat
                var fullAppt = await _ctx.Appointments
                    .AsNoTracking()
                    .Include(a => a.Service)
                    .Include(a => a.Customer)
                    .Include(a => a.Staff).ThenInclude(s => s!.User)
                    .Include(a => a.Review)
                    .FirstAsync(a => a.Id == appt.Id);

                return ApiResponse<AppointmentResponseDto>.Ok(MapToDto(fullAppt), "Dat lich thanh cong!");
            }
            finally
            {
                slotLock.Release();
            }
        }

        public async Task<ApiResponse<AppointmentResponseDto>> GuestCreateAsync(AppointmentCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.GuestName))
                return ApiResponse<AppointmentResponseDto>.Fail("Vui long nhap ho ten");
            if (string.IsNullOrWhiteSpace(dto.GuestPhone))
                return ApiResponse<AppointmentResponseDto>.Fail("Vui long nhap so dien thoai");

            var service = await _svcRepo.GetByIdAsync(dto.ServiceId);
            if (service == null || !service.IsActive)
                return ApiResponse<AppointmentResponseDto>.Fail("Dich vu khong ton tai hoac da ngung");

            var staff = await _staffRepo.GetByIdAsync(dto.StaffId);
            if (staff == null || !staff.IsAvailable)
                return ApiResponse<AppointmentResponseDto>.Fail("Stylist khong ton tai hoac da ngung nhan lich");

            var startTime = dto.AppointmentDate;
            if (startTime <= DateTime.Now)
                return ApiResponse<AppointmentResponseDto>.Fail("Vui long chon thoi gian trong tuong lai");

            var endTime = startTime.AddMinutes(service.DurationMinutes);

            var slotLock = GetStaffDateLock(dto.StaffId, startTime.Date);
            await slotLock.WaitAsync();
            try
            {
                if (await HasSlotConflictAsync(dto.StaffId, startTime, endTime))
                    return ApiResponse<AppointmentResponseDto>.Fail("Khung gio nay da co nguoi dat");

                var appt = new Appointment
                {
                    Id = Guid.NewGuid(),
                    StaffId = dto.StaffId,
                    ServiceId = dto.ServiceId,
                    AppointmentDate = startTime,
                    Status = AppointmentStatus.Pending,
                    BookingCode = GenerateBookingCode(),
                    Notes = dto.Notes?.Trim(),
                    IsGuestBooking = true,
                    GuestName = dto.GuestName.Trim(),
                    GuestPhone = dto.GuestPhone.Trim(),
                    GuestEmail = dto.GuestEmail?.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                await _apptRepo.AddAsync(appt);
                await _apptRepo.SaveChangesAsync();

                _ = _email.SendBookingConfirmationAsync(appt.Id);

                var fullAppt = await _ctx.Appointments
                    .AsNoTracking()
                    .Include(a => a.Service)
                    .Include(a => a.Staff).ThenInclude(s => s!.User)
                    .FirstAsync(a => a.Id == appt.Id);

                return ApiResponse<AppointmentResponseDto>.Ok(MapToDto(fullAppt), "Dat lich thanh cong!");
            }
            finally
            {
                slotLock.Release();
            }
        }

        public async Task<ApiResponse<object>> UpdateStatusAsync(Guid id, string newStatus, Guid requesterId, string role)
        {
            var appt = await _apptRepo.GetByIdAsync(id);
            if (appt == null)
                return ApiResponse<object>.Fail("Khong tim thay lich hen");

            if (!AppointmentStatus.CanTransition(appt.Status, newStatus))
                return ApiResponse<object>.Fail($"Khong the chuyen tu {appt.Status} sang {newStatus}");

            if (role == Roles.Customer)
            {
                if (appt.CustomerId != requesterId)
                    return ApiResponse<object>.Fail("Khong co quyen");
                if (newStatus != AppointmentStatus.Cancelled)
                    return ApiResponse<object>.Fail("Khach hang chi co the huy lich");
            }
            else if (role == Roles.Staff)
            {
                var staff = await _staffRepo.FindAsync(s => s.UserId == requesterId);
                if (staff.FirstOrDefault()?.Id != appt.StaffId)
                    return ApiResponse<object>.Fail("Khong co quyen");
            }

            appt.Status = newStatus;
            _apptRepo.Update(appt);
            await _apptRepo.SaveChangesAsync();

            _ = _email.SendStatusUpdateAsync(appt.Id, newStatus);

            return ApiResponse<object>.Ok(null, $"Cap nhat thanh {newStatus} thanh cong");
        }

        public async Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(Guid staffId, Guid serviceId, DateTime date)
        {
            var service = await _svcRepo.GetByIdAsync(serviceId);
            var duration = service?.DurationMinutes ?? 60;

            var workingHour = await _whRepo.FindAsync(w => w.StaffId == staffId && w.DayOfWeek == (int)date.DayOfWeek);
            var wh = workingHour.FirstOrDefault();

            var slots = new List<AvailableSlotDto>();
            if (wh == null)
                return slots;

            var dayStart = date.Date.Add(wh.StartTime);
            var dayEnd = date.Date.Add(wh.EndTime);
            var now = DateTime.Now;

            // Doc lich da dat tu DB moi nhat
            var bookedAppts = await _ctx.Appointments
                .AsNoTracking()
                .Include(a => a.Service)
                .Where(a => a.StaffId == staffId
                    && a.AppointmentDate.Date == date.Date
                    && a.Status != AppointmentStatus.Cancelled)
                .Select(a => new { a.AppointmentDate, Duration = a.Service != null ? a.Service.DurationMinutes : 60 })
                .ToListAsync();

            var start = dayStart < now ? now : dayStart;
            while (start.AddMinutes(duration) <= dayEnd)
            {
                var end = start.AddMinutes(duration);
                var isAvailable = !bookedAppts.Any(b =>
                    b.AppointmentDate < end && b.AppointmentDate.AddMinutes(b.Duration) > start);
                slots.Add(new AvailableSlotDto
                {
                    StartTime = start.TimeOfDay,
                    EndTime = end.TimeOfDay,
                    IsAvailable = isAvailable
                });
                start = start.AddMinutes(30);
            }

            return slots;
        }

        private async Task<bool> HasSlotConflictAsync(Guid staffId, DateTime start, DateTime end)
        {
            var conflict = await _ctx.Appointments
                .AsNoTracking()
                .AnyAsync(a =>
                    a.StaffId == staffId &&
                    a.AppointmentDate.Date == start.Date &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.AppointmentDate < end &&
                    a.AppointmentDate.AddMinutes(
                        (a.Service != null ? a.Service.DurationMinutes : 60)) > start);
            return conflict;
        }

        private static string GenerateBookingCode()
        {
            var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return "HS" + new string(Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        private static bool IsValidEmail(string email)
            => !string.IsNullOrWhiteSpace(email) && email.Contains('@') && email.Contains('.');

        private static SemaphoreSlim GetStaffDateLock(Guid staffId, DateTime date)
            => _locks.GetOrAdd($"{staffId}:{date:yyyyMMdd}", _ => new SemaphoreSlim(1, 1));

        private static AppointmentResponseDto MapToDto(Appointment a) => new()
        {
            Id = a.Id,
            BookingCode = a.BookingCode,
            ServiceId = a.ServiceId,
            StaffId = a.StaffId,
            Status = a.Status,
            AppointmentDate = a.AppointmentDate,
            Notes = a.Notes,
            CreatedAt = a.CreatedAt,
            ServiceName = a.Service?.Name ?? "",
            ServiceImageUrl = a.Service?.ImageUrl ?? "",
            ServicePrice = a.Service?.Price ?? 0,
            DurationMinutes = a.Service?.DurationMinutes ?? 0,
            CustomerName = a.IsGuestBooking ? a.GuestName ?? "" : a.Customer?.FullName ?? "",
            CustomerPhone = a.IsGuestBooking ? a.GuestPhone ?? "" : a.Customer?.Phone ?? "",
            StaffName = a.Staff?.User?.FullName ?? "",
            IsGuestBooking = a.IsGuestBooking,
            GuestName = a.GuestName,
            GuestPhone = a.GuestPhone,
            HasReview = a.Review != null,
        };
    }
}
