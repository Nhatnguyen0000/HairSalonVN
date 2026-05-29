using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HairSalonVN.API.Middleware
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string LogPath = Path.Combine(AppContext.BaseDirectory, "audit.log");

        public AuditLogMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path.Value ?? "";
            var now = DateTime.UtcNow;

            await _next(context);

            var statusCode = context.Response.StatusCode;
            var user = context.User.Identity?.Name ?? "anonymous";

            if (method is "POST" or "PUT" or "DELETE")
            {
                var logEntry = $"[{now:yyyy-MM-dd HH:mm:ss}] [{method}] {path} | User: {user} | Status: {statusCode}";
                try
                {
                    await File.AppendAllTextAsync(LogPath, logEntry + Environment.NewLine);
                }
                catch { }
            }
        }
    }
}
