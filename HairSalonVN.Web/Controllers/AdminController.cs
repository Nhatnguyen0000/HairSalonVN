using HairSalonVN.Web.Helpers;
using HairSalonVN.Web.Models.Admin;
using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Models.Service;
using HairSalonVN.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HairSalonVN.Web.Controllers
{
    [Controller]
    public class AdminController : Controller
    {
        private readonly BookingApiService _booking;
        private readonly ServiceApiService _services;
        private readonly PaymentApiService _payments;
        private readonly StaffApiService _staffApi;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan PendingCacheTtl = TimeSpan.FromSeconds(45);

        public AdminController(
            BookingApiService b, ServiceApiService s,
            PaymentApiService payments, StaffApiService staffApi,
            IMemoryCache cache)
        { _booking = b; _services = s; _payments = payments; _staffApi = staffApi; _cache = cache; }

        private bool IsAdmin() => SessionAuth.IsAdmin(HttpContext);

        private IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/Admin" });
        }

        private void SetAdminViewBag(string activeMenu, int pendingCount = 0)
        {
            ViewBag.DashboardRole = "Admin";
            ViewBag.ActiveMenu = activeMenu;
            ViewBag.PendingCount = pendingCount;
        }

        private static int CountPending(IEnumerable<AppointmentViewModel>? appts) =>
            appts?.Count(a => a.Status == "Pending") ?? 0;

        private async Task<int> GetPendingCountAsync()
        {
            return await _cache.GetOrCreateAsync("admin:pending", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = PendingCacheTtl;
                var appts = await _booking.GetAllAsync();
                return CountPending(appts?.Data);
            });
        }

        private void InvalidatePendingCache() => _cache.Remove("admin:pending");

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToLogin();

            var appts = await _booking.GetAllAsync();
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var apptList = appts?.Data?.ToList() ?? new List<AppointmentViewModel>();

            var completedThisMonth = apptList
                .Where(a => a.Status == "Completed" && a.AppointmentDate >= monthStart)
                .ToList();

            var vm = new DashboardViewModel
            {
                TotalAppointmentsToday = apptList.Count(a => a.AppointmentDate.Date == DateTime.Today),
                TotalCount = apptList.Count,
                PendingCount = apptList.Count(a => a.Status == "Pending"),
                CompletedCount = completedThisMonth.Count,
                RevenueThisMonth = completedThisMonth.Sum(a => a.ServicePrice),
                RecentAppointments = apptList.OrderByDescending(a => a.CreatedAt).Take(10),
            };

            SetAdminViewBag("dashboard", vm.PendingCount);
            return View(vm);
        }

        public async Task<IActionResult> Appointments()
        {
            if (!IsAdmin()) return RedirectToLogin();
            
            var appts = await _booking.GetAllAsync();
            var list = appts?.Data ?? new List<AppointmentViewModel>();
            SetAdminViewBag("appointments", CountPending(list));
            return View(list);
        }

        public async Task<IActionResult> Services()
        {
            if (!IsAdmin()) return RedirectToLogin();
            
            SetAdminViewBag("services", await GetPendingCountAsync());

            var r = await _services.GetAllAsync();
            return View(r?.Data ?? new List<ServiceCardViewModel>());
        }

        public async Task<IActionResult> Staff()
        {
            if (!IsAdmin()) return RedirectToLogin();

            SetAdminViewBag("staff", await GetPendingCountAsync());

            var r = await _services.GetAllStaffAsync();
            return View(r?.Data ?? new List<StaffSlotViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService([FromForm] ServiceManageViewModel vm)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền" });

            if (string.IsNullOrWhiteSpace(vm.Name) || vm.Price <= 0 || vm.DurationMinutes <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            var r = await _services.CreateAsync(vm);
            return Json(new { success = r?.Success == true, message = r?.Success == true ? "Thêm dịch vụ thành công!" : r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateService([FromForm] ServiceManageViewModel vm)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền" });

            if (vm.Id == Guid.Empty || string.IsNullOrWhiteSpace(vm.Name) || vm.Price <= 0 || vm.DurationMinutes <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            var r = await _services.UpdateAsync(vm.Id, vm);
            return Json(new { success = r?.Success == true, message = r?.Success == true ? "Cập nhật dịch vụ thành công!" : r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền" });

            var r = await _services.DeleteAsync(id);
            return Json(new { success = r?.Success == true, message = r?.Success == true ? "Xóa dịch vụ thành công!" : r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentStatus(Guid id, string status)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền" });

            var r = await _booking.UpdateStatusAsync(id, status);
            if (r?.Success == true) InvalidatePendingCache();
            return Json(new { success = r?.Success == true, message = r?.Success == true ? $"Cập nhật thành {status} thành công!" : r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra" });
        }

        public async Task<IActionResult> Payments()
        {
            if (!IsAdmin()) return RedirectToLogin();

            SetAdminViewBag("payments", await GetPendingCountAsync());
            var list = (await _payments.GetAllAsync())?.Data?.ToList() ?? new List<PaymentViewModel>();
            var summary = await _payments.GetSummaryAsync();

            var vm = new PaymentsPageViewModel
            {
                Payments = list,
                TotalPaid = list.Where(p => p.Status == "Paid").Sum(p => p.Amount),
                TotalThisMonth = summary?.Data?.TotalThisMonth ?? 0,
                CountThisMonth = summary?.Data?.CountThisMonth ?? 0,
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditStaffHours(Guid id)
        {
            if (!IsAdmin()) return RedirectToLogin();

            var r = await _staffApi.GetByIdAsync(id);
            if (r?.Success != true || r.Data is null)
            {
                TempData["Error"] = "Không tìm thấy nhân viên";
                return RedirectToAction("Staff");
            }

            var staff = r.Data;
            var dayLabels = new[] { "Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
            var existing = staff.WorkingHours?.ToDictionary(w => w.DayOfWeek) ?? new();

            var vm = new StaffHoursEditViewModel
            {
                StaffId = staff.StaffId,
                StaffName = staff.StaffName,
                Days = Enumerable.Range(0, 7).Select(d =>
                {
                    existing.TryGetValue(d, out var wh);
                    return new DayHourRow
                    {
                        DayOfWeek = d,
                        DayLabel = dayLabels[d],
                        StartTime = wh?.StartTime ?? "08:00",
                        EndTime = wh?.EndTime ?? "20:00",
                        IsOpen = wh != null,
                    };
                }).ToList(),
            };

            SetAdminViewBag("staff", await GetPendingCountAsync());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStaffHours([FromForm] StaffHoursEditViewModel vm)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền" });

            var hours = vm.Days
                .Where(d => d.IsOpen)
                .Select(d => new WorkingHourUpdateModel
                {
                    DayOfWeek = d.DayOfWeek,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                })
                .ToList();

            var r = await _staffApi.UpdateWorkingHoursAsync(vm.StaffId, hours);
            return Json(new { success = r?.Success == true, message = r?.Success == true ? "Cập nhật giờ làm việc thành công!" : r?.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra" });
        }

        public async Task<IActionResult> Reports()
        {
            if (!IsAdmin()) return RedirectToLogin();

            var apptList = (await _booking.GetAllAsync())?.Data?.ToList() ?? new List<AppointmentViewModel>();
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var monthAppts = apptList.Where(a => a.AppointmentDate >= monthStart).ToList();
            var completedMonth = monthAppts.Where(a => a.Status == "Completed").ToList();

            var vm = new ReportsViewModel
            {
                RevenueThisMonth = completedMonth.Sum(a => a.ServicePrice),
                RevenueAllTime = apptList.Where(a => a.Status == "Completed").Sum(a => a.ServicePrice),
                AppointmentsThisMonth = monthAppts.Count,
                CompletedThisMonth = completedMonth.Count,
                CancelledThisMonth = monthAppts.Count(a => a.Status == "Cancelled"),
                TotalCustomers = apptList
                    .Select(a => a.CustomerName)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count(),
                TopServices = completedMonth
                    .GroupBy(a => a.ServiceName)
                    .Select(g => new ServiceStatItem
                    {
                        Name = g.Key,
                        Count = g.Count(),
                        Revenue = g.Sum(x => x.ServicePrice)
                    })
                    .OrderByDescending(x => x.Revenue)
                    .Take(5),
                TopStaff = completedMonth
                    .GroupBy(a => a.StaffName)
                    .Select(g => new StaffStatItem
                    {
                        Name = g.Key,
                        CompletedCount = g.Count()
                    })
                    .OrderByDescending(x => x.CompletedCount)
                    .Take(5),
            };

            SetAdminViewBag("reports", CountPending(apptList));
            return View(vm);
        }
    }
}
