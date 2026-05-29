using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Services;
using Microsoft.AspNetCore.Mvc;
using HairSalonVN.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace HairSalonVN.Web.Controllers;

public class HomeController : Controller
{
    private readonly ServiceApiService _serviceApi;
    private readonly ReviewApiService _reviewApi;
    private readonly IMemoryCache _cache;
    private readonly ILogger<HomeController> _logger;
    private static readonly TimeSpan HomeCacheTtl = TimeSpan.FromMinutes(2);

    public HomeController(ServiceApiService serviceApi,
                          ReviewApiService reviewApi,
                          IMemoryCache cache,
                          ILogger<HomeController> logger)
    {
        _serviceApi = serviceApi;
        _reviewApi = reviewApi;
        _cache = cache;
        _logger = logger;
    }

    // ── GET / (Landing Page) ──────────────────────────────────
    public async Task<IActionResult> Index()
    {
        try
        {
            ViewBag.Services = await _cache.GetOrCreateAsync("home:services", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = HomeCacheTtl;
                var r = await _serviceApi.GetAllAsync();
                return r?.Success == true
                    ? r.Data?.Where(s => s.IsActive).Take(3).ToList()
                    : new List<ServiceCardViewModel>();
            });

            ViewBag.Reviews = await _cache.GetOrCreateAsync("home:reviews", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = HomeCacheTtl;
                try
                {
                    var r = await _reviewApi.GetAllAsync();
                    return r?.Success == true && r.Data != null
                        ? r.Data.Take(3).ToList()
                        : new List<ReviewViewModel>();
                }
                catch
                {
                    return new List<ReviewViewModel>();
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not load landing page data from API");
            ViewBag.Services = null;
            ViewBag.Reviews = null;
        }

        return View();
    }

    // ── GET /Home/Error ───────────────────────────────────────
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
        });
    }

    // ── GET /Home/About (optional) ────────────────────────────
    public IActionResult About() => View();

    // ── GET /Home/Contact (optional) ──────────────────────────
    public IActionResult Contact() => View();
}