using HairSalonVN.Web.Helpers;
using HairSalonVN.Web.Models.Auth;
using HairSalonVN.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonVN.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthApiService _auth;
        public AccountController(AuthApiService auth) => _auth = auth;

        // ── GET /Account/Login ────────────────────────────────────
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(
                HttpContext.Session.GetString("AccessToken")))
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // ── POST /Account/Login ───────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _auth.LoginAsync(vm.Email, vm.Password);
            if (result?.Success != true)
            {
                ModelState.AddModelError("",
                    result?.Errors.FirstOrDefault()
                    ?? "Đăng nhập thất bại");
                return View(vm);
            }

            // Lưu token vào Session
            HttpContext.Session.SetString("AccessToken",
                result.Data!.AccessToken);
            HttpContext.Session.SetString("RefreshToken",
                result.Data.RefreshToken);
            HttpContext.Session.SetString("UserRole",
                result.Data.Role);
            HttpContext.Session.SetString("UserName",
                result.Data.FullName);
            HttpContext.Session.SetString("UserId",
                result.Data.UserId.ToString());

            TempData["Success"] = $"Chào mừng, {result.Data.FullName}!";

            var safeReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/"; // FIXED: B001 — only redirect to local return URLs after login.

            // Redirect theo role
            return Redirect(result.Data.Role switch
            {
                "Admin" => "/Admin",
                "Staff" => "/Staff",
                _ => safeReturnUrl
            });
        }

        // ── GET /Account/Register ─────────────────────────────────
        [HttpGet]
        public IActionResult Register() => View();

        // ── POST /Account/Register ────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _auth.RegisterAsync(vm);
            if (result?.Success != true)
            {
                foreach (var err in result?.Errors ?? new())
                    ModelState.AddModelError("", err);
                return View(vm);
            }

            // Auto-login sau khi đăng ký
            HttpContext.Session.SetString("AccessToken",
                result.Data!.AccessToken);
            HttpContext.Session.SetString("RefreshToken",
                result.Data.RefreshToken);
            HttpContext.Session.SetString("UserRole", result.Data.Role);
            HttpContext.Session.SetString("UserName", result.Data.FullName);

            TempData["Success"] = "Đăng ký thành công! Chào mừng bạn.";
            return RedirectToAction("Index", "Home");
        }

        // ── GET /Account/Profile ──────────────────────────────────
        [HttpGet]
        public IActionResult Profile()
        {
            if (!SessionAuth.IsLoggedIn(HttpContext))
                return RedirectToAction("Login", new { returnUrl = "/Account/Profile" });

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng";
                return RedirectToAction("Login");
            }

            var fullName = HttpContext.Session.GetString("UserName") ?? "";
            var role = HttpContext.Session.GetString("UserRole") ?? "";
            var email = HttpContext.Session.GetString("UserEmail") ?? "";

            var vm = new ProfileViewModel
            {
                Id = userId,
                FullName = fullName,
                Email = email,
                Phone = "",
                Role = role
            };

            ViewBag.DashboardRole = role == "Admin" ? "Admin"
                : role == "Staff" ? "Staff" : "Customer";
            ViewBag.ActiveMenu = "profile";
            return View(vm);
        }

        // ── GET /Account/Profile/Details ──────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> ProfileDetails()
        {
            if (!SessionAuth.IsLoggedIn(HttpContext))
                return RedirectToAction("Login", new { returnUrl = "/Account/Profile" });

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng";
                return RedirectToAction("Login");
            }

            var result = await _auth.GetUserByIdAsync(userId);
            if (result?.Success != true || result.Data is null)
            {
                TempData["Error"] = result?.Errors?.FirstOrDefault() ?? "Không tải được hồ sơ";
                return RedirectToAction("Index", "Home");
            }

            var vm = new ProfileViewModel
            {
                Id = result.Data.Id,
                FullName = result.Data.FullName,
                Email = result.Data.Email,
                Phone = result.Data.Phone ?? "",
                Role = result.Data.Role
            };

            ViewBag.DashboardRole = result.Data.Role == "Admin" ? "Admin"
                : result.Data.Role == "Staff" ? "Staff" : "Customer";
            ViewBag.ActiveMenu = "profile";
            return View("Profile", vm);
        }

        // ── POST /Account/UpdateProfile ────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(Guid id, string fullName, string? phone)
        {
            if (!SessionAuth.IsLoggedIn(HttpContext))
                return RedirectToAction("Login", new { returnUrl = "/Account/Profile" });

            // Validate
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Trim().Length < 2)
            {
                TempData["Error"] = "Họ tên phải có ít nhất 2 ký tự";
                return RedirectToAction("Profile");
            }

            if (!string.IsNullOrWhiteSpace(phone) && phone.Trim().Length < 9)
            {
                TempData["Error"] = "Số điện thoại không hợp lệ";
                return RedirectToAction("Profile");
            }

            // Call API to update
            var updateResult = await _auth.UpdateProfileAsync(id, fullName.Trim(), phone?.Trim());

            if (updateResult?.Success == true)
            {
                TempData["Success"] = "Cập nhật hồ sơ thành công!";
                HttpContext.Session.SetString("UserName", fullName.Trim());
            }
            else
            {
                TempData["Error"] = updateResult?.Errors?.FirstOrDefault() ?? "Cập nhật thất bại";
            }

            return RedirectToAction("Profile");
        }

        // ── POST /Account/Logout ──────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var rt = HttpContext.Session.GetString("RefreshToken");
            if (!string.IsNullOrEmpty(rt))
                await _auth.LogoutAsync(rt);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
