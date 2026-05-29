using HairSalonVN.Web.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// ── 1. Logging ───────────────────────────────────────────────────
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Information);
}

// ── 2. Kestrel & URLs ────────────────────────────────────────────
builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenLocalhost(7126);
    opt.Limits.MaxConcurrentConnections = 100;
    opt.Limits.MaxConcurrentUpgradedConnections = 100;
    opt.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    opt.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

// ── 3. CSRF Anti-Forgery ─────────────────────────────────────────
builder.Services.AddAntiforgery(opt =>
{
    opt.Cookie.Name = ".HairSalon.Antiforgery";
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.SuppressXFrameOptionsHeader = false;
});

// ── 4. MVC Core ──────────────────────────────────────────────────
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression(o => o.EnableForHttps = true);
builder.Services.AddMemoryCache();

// ── 5. Session ────────────────────────────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(30);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.Cookie.Name = ".HairSalon.Session";
});

// ── 6. IHttpContextAccessor ───────────────────────────────────────
builder.Services.AddHttpContextAccessor();

// ── 7. API Base URL ──────────────────────────────────────────────
var apiBase = Environment.GetEnvironmentVariable("HairSalon__ApiBaseUrl")
    ?? builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException(
        "ApiSettings:BaseUrl is not configured in appsettings.json and "
        + "HairSalon__ApiBaseUrl environment variable is not set.");

var baseUri = new Uri(apiBase.TrimEnd('/') + "/");

// ── 8. Typed HttpClient Services ──────────────────────────────────
builder.Services.AddHttpClient<AuthApiService>(c =>
{
    c.BaseAddress = baseUri;
    c.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<BookingApiService>(c =>
{
    c.BaseAddress = baseUri;
    c.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<ServiceApiService>(c =>
{
    c.BaseAddress = baseUri;
    c.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<ReviewApiService>(c =>
{
    c.BaseAddress = baseUri;
    c.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<PaymentApiService>(c =>
{
    c.BaseAddress = baseUri;
    c.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<StaffApiService>(c =>
{
    c.BaseAddress = baseUri;
    c.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// ── Pipeline ─────────────────────────────────────────────────────
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== LUXE Hair Salon Web Starting ===");
logger.LogInformation("Environment: {Env}", app.Environment.EnvironmentName);
logger.LogInformation("URL: http://localhost:7126");
logger.LogInformation("API Base: {ApiBase}", baseUri);

// Global exception handler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Static files & compression
app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache busting headers
        ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        ctx.Context.Response.Headers["Pragma"] = "no-cache";
        ctx.Context.Response.Headers["Expires"] = "0";
    }
});

app.UseRouting();
app.UseAntiforgery();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


logger.LogInformation("=== LUXE Hair Salon Web Started Successfully ===");
app.Run();

// Make Program accessible for testing
public partial class Program { }
