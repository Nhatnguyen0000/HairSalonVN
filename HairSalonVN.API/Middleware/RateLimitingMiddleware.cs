using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HairSalonVN.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, ClientRequest> _clients = new();
        private const int MaxRequests = 100;
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

        public RateLimitingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = $"{clientId}:{context.Request.Path}";

            var now = DateTime.UtcNow;
            var client = _clients.AddOrUpdate(key,
                _ => new ClientRequest { Count = 1, WindowStart = now },
                (_, existing) =>
                {
                    if (now - existing.WindowStart > Window)
                    {
                        existing.Count = 1;
                        existing.WindowStart = now;
                    }
                    else
                    {
                        existing.Count++;
                    }
                    return existing;
                });

            if (client.Count > MaxRequests)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync(new { success = false, message = "Quá nhiều yêu cầu. Vui lòng thử lại sau." });
                return;
            }

            await _next(context);
        }

        private class ClientRequest
        {
            public int Count { get; set; }
            public DateTime WindowStart { get; set; }
        }
    }
}
