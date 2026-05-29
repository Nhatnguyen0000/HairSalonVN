using System.Globalization;
using HairSalonVN.Web.Helpers;
using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Services;
using Microsoft.AspNetCore.Mvc;
using HairSalonVN.Web.Models.Shared;
using HairSalonVN.Database;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.Web.Controllers
{
    [Controller]
    public class BookingController : Controller
    {
        private readonly BookingApiService _booking;
        private readonly ServiceApiService _services;
        private readonly ReviewApiService _reviews;
        private readonly IConfiguration _config;

        public BookingController(
            BookingApiService b, ServiceApiService s, ReviewApiService r, IConfiguration config)
        { _booking = b; _services = s; _reviews = r; _config = config; }

        // ── BƯỚC 1: Chọn dịch vụ ─────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var r = await _services.GetAllAsync();
            return View(r?.Data ?? new List<ServiceCardViewModel>());
        }

        // ── BƯỚC 2: Chọn stylist & khung giờ ─────────────────────
        [HttpGet]
        public async Task<IActionResult> SelectSlot(
            Guid serviceId, string date)
        {
            var svcR = await _services.GetByServiceAsync(serviceId);
            var staffR = await _services.GetStaffByServiceAsync(serviceId);
            var staffList = staffR?.Data?.ToList() ?? new();

            // Some API responses may populate `Id` instead of `StaffId`.
            // Normalize so view models always have `StaffId` set.
            foreach (var s in staffList)
            {
                if (s.StaffId == Guid.Empty && s.Id != Guid.Empty)
                    s.StaffId = s.Id;
            }

            // Fallback: if no staff returned for the service, try fetching all staff
            // to avoid empty UI when backend filtering fails.
            if (!staffList.Any())
            {
                var allStaffR = await _services.GetAllStaffAsync();
                var allList = allStaffR?.Data?.ToList() ?? new();
                foreach (var s in allList)
                {
                    if (s.StaffId == Guid.Empty && s.Id != Guid.Empty)
                        s.StaffId = s.Id;
                }
                staffList = allList;
            }

            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out var parsedDate))
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }

            ViewBag.ServiceId = serviceId;
            ViewBag.ServiceName = svcR?.Data?.Name ?? "";
            ViewBag.ServicePrice = svcR?.Data?.Price ?? 0;
            ViewBag.Duration = svcR?.Data?.DurationMinutes ?? 0;
            ViewBag.Date = date;
            ViewBag.ApiBaseUrl = _services.BaseUrl;
            return View(staffList);
        }

        // ── AJAX: Lấy slot trống (gọi từ JS) ─────────────────────
        [HttpGet]
        public async Task<IActionResult> GetSlots(
            [FromQuery] Guid staffId,
            [FromQuery] Guid serviceId,
            [FromQuery] string date)
        {
            try
            {
                // Parse date từ string vì JS gửi format yyyy-MM-dd
                if (!DateTime.TryParse(date, out var parsedDate))
                {
                    return Json(new { success = false, message = "Ngày không hợp lệ." });
                }

                // Gọi trực tiếp database để lấy slots
                var connStr = _config.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connStr))
                {
                    return Json(new { success = false, message = "Không có kết nối database." });
                }

                var slots = await GetAvailableSlotsFromDbAsync(staffId, serviceId, parsedDate.Date, connStr);
                return Json(new { success = true, data = slots });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        private async Task<List<object>> GetAvailableSlotsFromDbAsync(Guid staffId, Guid serviceId, DateTime date, string connStr)
        {
            var slots = new List<object>();

            await using var conn = new Microsoft.Data.SqlClient.SqlConnection(connStr);
            await conn.OpenAsync();

            // Lấy duration của service
            int duration = 60;
            using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(
                "SELECT DurationMinutes FROM Services WHERE Id = @ServiceId", conn))
            {
                cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                    duration = Convert.ToInt32(result);
            }

            // Lấy các lịch hẹn đã đặt trong ngày
            var bookedTimes = new List<DateTime>();
            using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
                SELECT a.AppointmentDate, s.DurationMinutes 
                FROM Appointments a 
                JOIN Services s ON a.ServiceId = s.Id 
                WHERE a.StaffId = @StaffId 
                  AND CAST(a.AppointmentDate AS DATE) = CAST(@Date AS DATE)
                  AND a.Status NOT IN ('Cancelled', 'NoShow')", conn))
            {
                cmd.Parameters.AddWithValue("@StaffId", staffId);
                cmd.Parameters.AddWithValue("@Date", date);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var apptTime = reader.GetDateTime(0);
                    var apptDuration = reader.IsDBNull(1) ? 60 : reader.GetInt32(1);
                    bookedTimes.Add(apptTime);
                }
            }

            // Tạo slots từ 8h-16h
            var dayStart = date.Date.AddHours(8);
            var dayEnd = date.Date.AddHours(16);
            var now = DateTime.Now;

            var start = dayStart < now ? now : dayStart;
            if (start.Minute % 30 != 0)
            {
                start = start.AddMinutes(30 - start.Minute % 30);
            }

            while (start.AddMinutes(duration) <= dayEnd)
            {
                var end = start.AddMinutes(duration);
                var isAvailable = !bookedTimes.Any(b => b < end && b.AddMinutes(duration) > start);

                slots.Add(new
                {
                    startTime = start.TimeOfDay.ToString(@"hh\:mm\:ss"),
                    endTime = end.TimeOfDay.ToString(@"hh\:mm\:ss"),
                    label = start.ToString("HH:mm"),
                    time = start.ToString("HH:mm"),
                    isAvail = isAvailable,
                    isAvailable = isAvailable,
                    isAvailBool = isAvailable
                });

                start = start.AddMinutes(30);
            }

            return slots;
        }

        // ── BƯỚC 3: Trang xác nhận (GET) ────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Confirm(
            Guid serviceId, Guid staffId, string date, string time)
        {
            try
            {
                if (serviceId == Guid.Empty || staffId == Guid.Empty
                    || string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(time))
                {
                    TempData["Error"] = "Vui lòng chọn stylist và khung giờ.";
                    return RedirectToAction(nameof(SelectSlot),
                        new { serviceId, date = date ?? DateTime.Today.ToString("yyyy-MM-dd") });
                }

                if (!DateTime.TryParse(date, out var dateOnly))
                {
                    TempData["Error"] = "Ngày không hợp lệ.";
                    return RedirectToAction(nameof(SelectSlot), new { serviceId });
                }

                var timeText = time?.Trim() ?? string.Empty;
                if (!TryParseSlotTime(timeText, out var timeOfDay))
                {
                    TempData["Error"] = "Khung giờ không hợp lệ.";
                    return RedirectToAction(nameof(SelectSlot), new { serviceId, date });
                }

                var appointmentDate = dateOnly.Date.Add(timeOfDay);
                var vm = await BuildConfirmViewModelAsync(serviceId, staffId, appointmentDate);
                SetConfirmViewBags(vm);
                return View(vm);
            }
            catch (Exception)
            {
                // Defensive: avoid showing an empty/blank page on unexpected errors.
                TempData["Error"] = "Đã có lỗi xảy ra khi chuẩn bị trang xác nhận. Vui lòng thử lại.";
                return RedirectToAction(nameof(SelectSlot), new { serviceId, date = date ?? DateTime.Today.ToString("yyyy-MM-dd") });
            }
        }

        // ── POST: Gửi đặt lịch ───────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(AppointmentCreateViewModel vm)
        {
            try
            {
                var isLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("AccessToken"));
            vm.IsGuestBooking = !isLoggedIn;
            // Ensure AppointmentDate and StaffId are populated from form if model binding failed
            if (vm.AppointmentDate == default)
            {
                var rawDate = Request.Form["AppointmentDate"].ToString();
                if (!string.IsNullOrWhiteSpace(rawDate) && DateTime.TryParse(rawDate, out var parsed))
                    vm.AppointmentDate = parsed;
            }

            if (vm.StaffId == Guid.Empty)
            {
                var rawStaff = Request.Form["StaffId"].ToString();
                if (!string.IsNullOrWhiteSpace(rawStaff) && Guid.TryParse(rawStaff, out var parsedSid))
                    vm.StaffId = parsedSid;
            }

            // If still missing StaffId or other display fields, refresh from service to fill defaults
            if (vm.StaffId == Guid.Empty || string.IsNullOrEmpty(vm.ServiceName) || string.IsNullOrEmpty(vm.StaffName))
            {
                var refreshed = await BuildConfirmViewModelAsync(vm.ServiceId, vm.StaffId, vm.AppointmentDate);
                // copy only the fields we need for creation/display
                vm.StaffId = refreshed.StaffId;
                vm.ServiceName = refreshed.ServiceName;
                vm.ServicePrice = refreshed.ServicePrice;
                vm.Duration = refreshed.Duration;
                vm.StaffName = refreshed.StaffName;
            }

            if (!isLoggedIn)
            {
                if (string.IsNullOrWhiteSpace(vm.GuestName))
                    ModelState.AddModelError("GuestName", "Vui lòng nhập họ tên");
                if (string.IsNullOrWhiteSpace(vm.GuestPhone))
                    ModelState.AddModelError("GuestPhone", "Vui lòng nhập số điện thoại");
                else if (vm.GuestPhone.Length < 10)
                    ModelState.AddModelError("GuestPhone", "Số điện thoại không hợp lệ");
            }

                if (!ModelState.IsValid)
                {
                    // Log model state errors for diagnosis (server-side) and show to user
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                    if (errors.Any())
                        TempData["Error"] = string.Join("; ", errors);
                    // Ensure view displays service/staff info even when model binding failed
                    vm = await RefreshConfirmAsync(vm);
                    SetConfirmViewBags(vm);
                    // Preserve model errors in ModelState (already present)
                    return View(vm);
                }

            // Defensive checks before calling API
            if (vm.ServiceId == Guid.Empty)
            {
                ModelState.AddModelError("", "Dịch vụ không hợp lệ.");
                vm = await RefreshConfirmAsync(vm);
                SetConfirmViewBags(vm);
                return View(vm);
            }
            if (vm.StaffId == Guid.Empty)
            {
                ModelState.AddModelError("", "Stylist không hợp lệ.");
                vm = await RefreshConfirmAsync(vm);
                SetConfirmViewBags(vm);
                return View(vm);
            }
            if (vm.AppointmentDate == default)
            {
                ModelState.AddModelError("", "Ngày/giờ không hợp lệ.");
                vm = await RefreshConfirmAsync(vm);
                SetConfirmViewBags(vm);
                return View(vm);
            }

            ApiResponse<AppointmentViewModel>? r;
            // Record whether an access token is present in session (do not expose token value)
            try
            {
                TempData["HasAccessToken"] = !string.IsNullOrEmpty(HttpContext.Session.GetString("AccessToken"));
            }
            catch { TempData["HasAccessToken"] = false; }

            if (isLoggedIn)
            {
                r = await _booking.CreateAsync(vm);
            }
            else
            {
                r = await _booking.GuestCreateAsync(vm);
            }

            if (r?.Success == true)
            {
                // Provide booking details to Success page for invoice display
                TempData["BookingService"] = r.Data!.ServiceName;
                TempData["BookingStaff"] = r.Data.StaffName;
                TempData["BookingDate"] = r.Data.AppointmentDate.ToString("dd/MM/yyyy");
                TempData["BookingTime"] = r.Data.AppointmentDate.ToString("HH:mm");
                return RedirectToAction("Success",
                    new { code = r.Data.BookingCode });
            }
            // Show detailed API error when available
            var apiMsg = r?.Message;
            var apiErr = r?.Errors?.FirstOrDefault();
            string finalMsg;
            if (r == null)
            {
                finalMsg = "Không thể kết nối đến dịch vụ đặt lịch. Vui lòng thử lại.";
            }
            else if (!string.IsNullOrWhiteSpace(apiErr))
            {
                // Sanitize: truncate long API errors and strip HTML tags to prevent XSS
                var sanitized = System.Text.RegularExpressions.Regex.Replace(apiErr!, "<[^>]+>", "");
                finalMsg = sanitized.Length > 200 ? sanitized[..200] + "..." : sanitized;
            }
            else if (!string.IsNullOrWhiteSpace(apiMsg))
            {
                var sanitized = System.Text.RegularExpressions.Regex.Replace(apiMsg!, "<[^>]+>", "");
                finalMsg = sanitized.Length > 200 ? sanitized[..200] + "..." : sanitized;
            }
            else
            {
                finalMsg = "Đặt lịch thất bại";
            }
            ModelState.AddModelError("", finalMsg);
            TempData["Error"] = finalMsg;

            // Re-populate display fields before returning to view
            vm = await RefreshConfirmAsync(vm);
            SetConfirmViewBags(vm);
            return View(vm);
            }
            catch (Exception)
            {
                // Unexpected error: avoid returning blank page — redirect back with message
                TempData["Error"] = "Đã có lỗi xảy ra khi gửi yêu cầu đặt lịch. Vui lòng thử lại.";
                return RedirectToAction(nameof(SelectSlot), new { serviceId = vm.ServiceId, date = vm.AppointmentDate == default ? DateTime.Today.ToString("yyyy-MM-dd") : vm.AppointmentDate.ToString("yyyy-MM-dd") });
            }
        }

        // ── Trang thành công ──────────────────────────────────────
        public IActionResult Success(string code)
        {
            ViewBag.BookingCode = code;
            return View();
        }

        // ── Lịch hẹn của tôi (Customer) ─────────────────────────────────────
        public async Task<IActionResult> MyBookings()
        {
            if (!SessionAuth.IsLoggedIn(HttpContext))
                return RedirectToAction("Login", "Account");

            var r = await _booking.GetMyAsync();
            var appointments = r?.Data?.ToList() ?? new List<AppointmentViewModel>();

            return View(new MyBookingsViewModel
            {
                Appointments = appointments,
                TotalCount = appointments.Count,
                PendingCount = appointments.Count(a => a.Status == "Pending"),
                ConfirmedCount = appointments.Count(a => a.Status == "Confirmed"),
                CompletedCount = appointments.Count(a => a.Status == "Completed"),
                CancelledCount = appointments.Count(a => a.Status == "Cancelled"),
            });
        }

        // ── POST: Hủy lịch ─────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            if (!SessionAuth.IsLoggedIn(HttpContext))
                return RedirectToAction("Login", "Account", new { returnUrl = "/Booking/MyBookings" }); // FIXED: B015 — block anonymous cancel requests at MVC layer.

            var r = await _booking.UpdateStatusAsync(id, "Cancelled");
            TempData[r?.Success == true ? "Success" : "Error"] =
                r?.Success == true ? "Đã hủy lịch hẹn" : r?.Errors?.FirstOrDefault();
            return RedirectToAction("MyBookings");
        }

        // ── POST: Tạo đánh giá (JSON) ──────────────────────────────
        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromBody] CreateReviewViewModel vm)
        {
            if (!SessionAuth.IsLoggedIn(HttpContext))
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            if (vm == null || vm.Rating < 1 || vm.Rating > 5)
                return Json(new { success = false, message = "Điểm đánh giá phải từ 1 đến 5 sao" });

            var r = await _reviews.CreateAsync(vm);

            if (r?.Success == true)
                return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá!" });

            return Json(new { success = false, message = r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra" });
        }

        private async Task<AppointmentCreateViewModel> BuildConfirmViewModelAsync(
            Guid serviceId, Guid staffId, DateTime appointmentDate)
        {
            var svcR = await _services.GetByServiceAsync(serviceId);
            var staffR = await _services.GetStaffByServiceAsync(serviceId);
            var staff = staffR?.Data?.FirstOrDefault(s => s.StaffId == staffId)
                ?? staffR?.Data?.FirstOrDefault();

            if (staff != null && staff.StaffId != Guid.Empty)
                staffId = staff.StaffId;

            return new AppointmentCreateViewModel
            {
                ServiceId = serviceId,
                StaffId = staffId,
                AppointmentDate = appointmentDate,
                ServiceName = svcR?.Data?.Name ?? "",
                ServicePrice = svcR?.Data?.Price ?? 0,
                Duration = svcR?.Data?.DurationMinutes ?? 0,
                StaffName = staff?.StaffName ?? ""
            };
        }

        private void SetConfirmViewBags(AppointmentCreateViewModel vm)
        {
            ViewBag.ServiceName = vm.ServiceName;
            ViewBag.ServicePrice = vm.ServicePrice;
            ViewBag.Duration = vm.Duration;
            ViewBag.StaffName = vm.StaffName;
        }

        private async Task<AppointmentCreateViewModel> RefreshConfirmAsync(AppointmentCreateViewModel vm)
        {
            var refreshed = await BuildConfirmViewModelAsync(
                vm.ServiceId, vm.StaffId, vm.AppointmentDate);
            refreshed.Notes = vm.Notes;
            refreshed.GuestName = vm.GuestName;
            refreshed.GuestPhone = vm.GuestPhone;
            refreshed.GuestEmail = vm.GuestEmail;
            refreshed.IsGuestBooking = vm.IsGuestBooking;
            // Preserve display fields (may have been reset by model binding)
            if (string.IsNullOrEmpty(refreshed.ServiceName))
                refreshed.ServiceName = vm.ServiceName;
            if (string.IsNullOrEmpty(refreshed.StaffName))
                refreshed.StaffName = vm.StaffName;
            if (refreshed.ServicePrice == 0)
                refreshed.ServicePrice = vm.ServicePrice;
            if (refreshed.Duration == 0)
                refreshed.Duration = vm.Duration;
            return refreshed;
        }

        private static bool TryParseSlotTime(string timeText, out TimeSpan timeOfDay)
        {
            timeText = timeText.Trim();
            
            // Try parsing as DateTime first (most reliable for "HH:mm" format)
            if (DateTime.TryParseExact(timeText, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                timeOfDay = dt.TimeOfDay;
                return true;
            }
            
            // Try parsing with DateTime and "H:mm" format
            if (DateTime.TryParseExact(timeText, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                timeOfDay = dt.TimeOfDay;
                return true;
            }
            
            // Try standard TimeSpan parsing
            if (TimeSpan.TryParse(timeText, CultureInfo.InvariantCulture, out timeOfDay))
                return true;

            return TimeSpan.TryParseExact(
                timeText,
                new[] { @"hh\:mm", @"h\:mm", @"HH\:mm" },
                CultureInfo.InvariantCulture,
                out timeOfDay);
        }
    }

}
