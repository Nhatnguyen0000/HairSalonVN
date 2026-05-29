using HairSalonVN.Web.Helpers;
using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Models.Staff;
using HairSalonVN.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonVN.Web.Controllers
{
    [Controller]
    public class StaffController : Controller
    {
        private readonly BookingApiService _booking;
        private readonly StaffApiService _staffApi;

        public StaffController(BookingApiService booking, StaffApiService staffApi)
        {
            _booking = booking;
            _staffApi = staffApi;
        }

        private bool IsStaff() => SessionAuth.IsStaff(HttpContext);

        private IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/Staff" });
        }

        private void SetStaffViewBag(string activeMenu = "dashboard")
        {
            ViewBag.DashboardRole = "Staff";
            ViewBag.ActiveMenu = activeMenu;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsStaff()) return RedirectToLogin();

            var myShiftAppts = await _booking.GetMyShiftAsync();
            var appointments = myShiftAppts?.Data?.ToList() ?? new List<AppointmentViewModel>();

            var today = DateTime.Today;
            var now = DateTime.Now; // FIXED: B016 — compare staff dashboard appointments in local time.

            var vm = new StaffDashboardViewModel
            {
                TodayAppointments = appointments
                    .Where(a => a.AppointmentDate.Date == today && a.Status != "Cancelled")
                    .OrderBy(a => a.AppointmentDate)
                    .ToList(),
                UpcomingAppointments = appointments
                    .Where(a => a.AppointmentDate.Date >= today && a.AppointmentDate > now && a.Status != "Cancelled")
                    .OrderBy(a => a.AppointmentDate)
                    .Take(10)
                    .ToList(),
                AllAppointments = appointments
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList(),
                CompletedToday = appointments
                    .Count(a => a.AppointmentDate.Date == today && a.Status == "Completed"),
                PendingCount = appointments
                    .Count(a => a.AppointmentDate.Date == today && (a.Status == "Pending" || a.Status == "Confirmed")),
                TotalToday = appointments
                    .Count(a => a.AppointmentDate.Date == today && a.Status != "Cancelled"),
                UserName = HttpContext.Session.GetString("UserName") ?? "Nhân viên"
            };

            SetStaffViewBag("dashboard");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            if (!IsStaff()) return Forbid();

            var r = await _booking.UpdateStatusAsync(id, status);
            TempData[r?.Success == true ? "Success" : "Error"] =
                r?.Success == true ? $"Cập nhật thành {status} thành công!"
                                 : r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MyBookings()
        {
            if (!IsStaff()) return RedirectToLogin();

            var myShiftAppts = await _booking.GetMyShiftAsync();
            var appointments = myShiftAppts?.Data?.ToList() ?? new List<AppointmentViewModel>();

            var vm = new StaffMyBookingsViewModel
            {
                AllAppointments = appointments
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList(),
                CompletedCount = appointments.Count(a => a.Status == "Completed"),
                CancelledCount = appointments.Count(a => a.Status == "Cancelled"),
                PendingCount = appointments.Count(a => a.Status == "Pending"),
                ConfirmedCount = appointments.Count(a => a.Status == "Confirmed"),
                UserName = HttpContext.Session.GetString("UserName") ?? "Stylist"
            };

            SetStaffViewBag("staffhistory");
            return View(vm);
        }

        public async Task<IActionResult> Profile()
        {
            if (!IsStaff()) return RedirectToLogin();

            var r = await _staffApi.GetMeAsync();
            if (r?.Success != true || r.Data is null)
            {
                TempData["Error"] = r?.Errors?.FirstOrDefault() ?? "Không tải được hồ sơ stylist";
                return RedirectToAction("Index");
            }

            SetStaffViewBag("profile");
            return View(r.Data);
        }
    }
}
