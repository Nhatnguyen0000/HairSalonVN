        using HairSalonVN.Web.Models.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace HairSalonVN.Web.Services;

public abstract class ApiClientBase
{
    protected readonly HttpClient _http;
    protected readonly IHttpContextAccessor _ctx;
    private readonly ILogger? _logger;
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string BaseUrl => _http.BaseAddress?.ToString().TrimEnd('/') ?? "";

    protected ApiClientBase(HttpClient http, IHttpContextAccessor ctx, ILogger? logger = null)
    { _http = http; _ctx = ctx; _logger = logger; }

    protected void AttachToken()
    {
        var token = _ctx.HttpContext?.Session.GetString("AccessToken");
        if (string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    protected async Task<ApiResponse<T>?> GetAsync<T>(string url)
    {
        AttachToken();
        try
        {
            var res = await _http.GetAsync(url);
            _logger?.LogInformation("[API] GET {Url} -> {StatusCode}", url, res.StatusCode);
            if (!res.IsSuccessStatusCode)
            {
                _logger?.LogWarning("[API] GET {Url} failed with {StatusCode}", url, res.StatusCode);
                var text = await res.Content.ReadAsStringAsync();
                // Try parse error JSON
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResponse<T>>(text, JsonOpts);
                    if (parsed != null) return parsed;
                }
                catch { }
                var msg = string.IsNullOrWhiteSpace(text)
                    ? $"{(int)res.StatusCode} {res.StatusCode} (no response body)"
                    : text;
                return new ApiResponse<T> { Success = false, Message = msg, Errors = new List<string> { msg } };
            }
            try
            {
                return await res.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOpts);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "[API] GET {Url} failed to deserialize JSON", url);
                var fallback = await res.Content.ReadAsStringAsync();
                return new ApiResponse<T> { Success = false, Message = fallback };
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[API] GET {Url} threw exception", url);
            return null;
        }
    }

    protected async Task<ApiResponse<T>?> PostAsync<T>(string url, object body)
    {
        AttachToken();
        try
        {
            var res = await _http.PostAsJsonAsync(url, body);
            _logger?.LogInformation("[API] POST {Url} -> {StatusCode}", url, res.StatusCode);
            if (!res.IsSuccessStatusCode)
            {
                _logger?.LogWarning("[API] POST {Url} failed with {StatusCode}", url, res.StatusCode);
                var text = await res.Content.ReadAsStringAsync();
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResponse<T>>(text, JsonOpts);
                    if (parsed != null) return parsed;
                }
                catch { }
                // Include status code in message to aid debugging when API returns no body
                var msg = string.IsNullOrWhiteSpace(text)
                    ? $"{(int)res.StatusCode} {res.StatusCode} (no response body)"
                    : text;
                return new ApiResponse<T> { Success = false, Message = msg, Errors = new List<string> { msg } };
            }
            try
            {
                return await res.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOpts);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "[API] POST {Url} failed to deserialize JSON", url);
                var fallback = await res.Content.ReadAsStringAsync();
                return new ApiResponse<T> { Success = false, Message = fallback };
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[API] POST {Url} threw exception", url);
            return null;
        }
    }

    protected async Task<ApiResponse<T>?> PutAsync<T>(string url, object body)
    {
        AttachToken();
        try
        {
            var res = await _http.PutAsJsonAsync(url, body);
            _logger?.LogInformation("[API] PUT {Url} -> {StatusCode}", url, res.StatusCode);
            var text = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
            {
                _logger?.LogWarning("[API] PUT {Url} failed with {StatusCode}", url, res.StatusCode);
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResponse<T>>(text, JsonOpts);
                    if (parsed != null) return parsed;
                }
                catch { }
                return new ApiResponse<T> { Success = false, Message = text, Errors = new List<string> { text } };
            }

            return JsonSerializer.Deserialize<ApiResponse<T>>(text, JsonOpts);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[API] PUT {Url} threw exception", url);
            return new ApiResponse<T> { Success = false, Message = ex.Message, Errors = new List<string> { ex.Message } }; // FIXED: B014 — return logged API errors instead of swallowing PUT failures.
        }
    }

    protected async Task<ApiResponse<T>?> DeleteAsync<T>(string url)
    {
        AttachToken();
        try
        {
            var res = await _http.DeleteAsync(url);
            _logger?.LogInformation("[API] DELETE {Url} -> {StatusCode}", url, res.StatusCode);
            var text = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
            {
                _logger?.LogWarning("[API] DELETE {Url} failed with {StatusCode}", url, res.StatusCode);
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResponse<T>>(text, JsonOpts);
                    if (parsed != null) return parsed;
                }
                catch { }
                return new ApiResponse<T> { Success = false, Message = text, Errors = new List<string> { text } };
            }

            return JsonSerializer.Deserialize<ApiResponse<T>>(text, JsonOpts);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[API] DELETE {Url} threw exception", url);
            return new ApiResponse<T> { Success = false, Message = ex.Message, Errors = new List<string> { ex.Message } }; // FIXED: B014 — return logged API errors instead of swallowing DELETE failures.
        }
    }
}
