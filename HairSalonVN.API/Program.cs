using HairSalonVN.API.Middleware;
using HairSalonVN.API.Helpers;
using HairSalonVN.API.Services;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database;
using HairSalonVN.Database.Constants;
using HairSalonVN.Database.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
if (builder.Environment.IsDevelopment())
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
else
    builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenLocalhost(7098);
    opt.Limits.MaxConcurrentConnections = 100;
    opt.Limits.MaxConcurrentUpgradedConnections = 100;
    opt.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    opt.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

var allowedOrigins = builder.Environment.IsDevelopment()
    ? new[] { "http://localhost:5000", "http://localhost:5001", "http://localhost:7126", "http://localhost:5173", "http://127.0.0.1:7126" }
    : Array.Empty<string>();

builder.Services.AddDbContext<SalonDbContext>(opt =>
{
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(3));
    if (builder.Environment.IsDevelopment())
    {
        opt.EnableSensitiveDataLogging();
        opt.EnableDetailedErrors();
    }
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServiceService, ServiceManagementService>();
builder.Services.AddScoped<IStaffService, StaffManagementService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddScoped<IAppointmentService>(sp =>
{
    var ctx = sp.GetRequiredService<SalonDbContext>();
    var email = sp.GetRequiredService<EmailService>();
    return new AppointmentService(
        sp.GetRequiredService<IRepository<Appointment>>(),
        sp.GetRequiredService<IRepository<Service>>(),
        sp.GetRequiredService<IRepository<WorkingHour>>(),
        sp.GetRequiredService<IRepository<Staff>>(),
        ctx,
        email);
});

builder.Services.AddSingleton<EmailBackgroundHandler>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<EmailBackgroundHandler>());

// --- BO QUA JWT, TEST NHANH ---
builder.Services.AddSingleton<JwtHelper>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        // Tat tat ca validation - cho phep moi request
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            RequireSignedTokens = false,
            SignatureValidator = (token, parameters) => new JwtSecurityToken(token)
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminOnly", p => p.RequireRole(Roles.Admin));
    opt.AddPolicy("StaffOnly", p => p.RequireRole(Roles.Staff));
    opt.AddPolicy("StaffAdmin", p => p.RequireRole(Roles.Admin, Roles.Staff));
    opt.AddPolicy("CustomerOnly", p => p.RequireRole(Roles.Customer));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LUXE Hair Salon API",
        Version = "v1",
        Description = "REST API - Test version (JWT disabled)"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token (khong bat buoc)"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Output: camelCase (Frontend expects camelCase)
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        // Input: Accept both PascalCase (from Web/ASP.NET) and camelCase
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // Allow exception details for debugging
        o.AllowInputFormatterExceptionMessages = true;
    });

builder.Services.AddScoped<RequestLoggingFilter>();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== LUXE Hair Salon API Starting (TEST MODE - JWT DISABLED) ===");
logger.LogInformation("Environment: {Env}", app.Environment.EnvironmentName);
logger.LogInformation("URL: http://localhost:7098");

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<AuditLogMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    ctx.Response.Headers.Append("X-Frame-Options", "DENY");
    ctx.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    ctx.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalonDbContext>();
    try
    {
        var canConnect = await db.Database.CanConnectAsync();
        if (canConnect)
            logger.LogInformation("Database connected successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database connection failed.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LUXE Hair Salon API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors(p => p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

logger.LogInformation("=== API Ready - Test ngay tren Swagger! ===");
logger.LogInformation("  GET  /api/appointments       - Danh sach lich hen");
logger.LogInformation("  GET  /api/services          - Danh sach dich vu");
logger.LogInformation("  GET  /api/staff             - Danh sach nhan vien");
logger.LogInformation("  GET  /api/reviews           - Danh sach danh gia");
logger.LogInformation("  POST /api/appointments      - Tao lich hen");
logger.LogInformation("  POST /api/appointments/guest - Dat lich khach");

app.Run();

public partial class Program { }
