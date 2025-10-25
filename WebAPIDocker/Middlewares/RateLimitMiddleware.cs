using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;

namespace WebAPIDocker.Middlewares;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    
    // Dùng ConcurrentDictionary để lưu số request theo IP
    private static readonly ConcurrentDictionary<string, RateLimitEntry> _clients = new();

    private readonly int _limit;
    private readonly TimeSpan _window;
    
    
    public RateLimitMiddleware(RequestDelegate next, int limit = 5, int windowSeconds = 10)
    {
        _next = next;
        _limit = limit;
        _window = TimeSpan.FromSeconds(windowSeconds);
    }
    
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        var entry = _clients.GetOrAdd(ip, _ => new RateLimitEntry
        {
            Count = 0,
            WindowStart = now
        });

        var sem = _locks.GetOrAdd(ip, _ => new SemaphoreSlim(1, 1));

        await sem.WaitAsync(); // async-safe
        try
        {
            if (now - entry.WindowStart > _window)
            {
                entry.Count = 0;
                entry.WindowStart = now;
            }

            entry.Count++;

            if (entry.Count > _limit)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                var retryAfter = (_window - (now - entry.WindowStart)).TotalSeconds;
                context.Response.Headers["Retry-After"] = retryAfter.ToString("F0");

                var result = JsonSerializer.Serialize(new
                {
                    status = 429,
                    error = "Too many requests",
                    message = $"Bạn đã vượt quá {_limit} request trong {_window.TotalSeconds} giây. Vui lòng thử lại sau."
                });

                await context.Response.WriteAsync(result); // OK
                return;
            }
        }
        finally
        {
            sem.Release();
        }

        await _next(context);
    }


    
    private class RateLimitEntry
    {
        public int Count { get; set; }
        public DateTime WindowStart { get; set; }
    }

    
}