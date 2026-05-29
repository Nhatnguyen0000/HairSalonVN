using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Models.Shared;
using Microsoft.Extensions.Logging;

namespace HairSalonVN.Web.Services
{
    public class StaffApiService : ApiClientBase
    {
        private readonly ILogger<StaffApiService>? _logger;

        public StaffApiService(HttpClient http, IHttpContextAccessor ctx, ILogger<StaffApiService>? logger = null)
            : base(http, ctx)
        { _logger = logger; }

        public Task<ApiResponse<StaffDetailViewModel>?> GetMeAsync()
            => GetAsync<StaffDetailViewModel>("staff/me");

        public Task<ApiResponse<StaffDetailViewModel>?> GetByIdAsync(Guid id)
            => GetAsync<StaffDetailViewModel>($"staff/{id}");

        public Task<ApiResponse<object>?> UpdateWorkingHoursAsync(
            Guid staffId, IEnumerable<WorkingHourUpdateModel> hours)
            => PutAsync<object>($"staff/{staffId}/working-hours",
                hours.Select(h => new
                {
                    h.DayOfWeek,
                    StartTime = ParseTime(h.StartTime, "StartTime"),
                    EndTime = ParseTime(h.EndTime, "EndTime")
                }));

        private TimeSpan ParseTime(string value, string fieldName)
        {
            if (TimeSpan.TryParse(value, out var ts))
                return ts;
            if (value.Length == 5 && value[2] == ':')
                return TimeSpan.Parse(value + ":00");
            _logger?.LogWarning("Invalid time value for {Field}: '{Value}'", fieldName, value);
            throw new FormatException($"{fieldName} không hợp lệ"); // FIXED: B020 — do not silently save invalid working-hour values as 08:00.
        }
    }

    public class WorkingHourUpdateModel
    {
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; } = "08:00";
        public string EndTime { get; set; } = "20:00";
    }
}
