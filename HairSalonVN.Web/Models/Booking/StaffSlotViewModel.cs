using System.Text.Json.Serialization;

namespace HairSalonVN.Web.Models.Booking
{
    public class StaffSlotViewModel
    {
        [JsonPropertyName("staffId")]
        public Guid StaffId { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public List<string>? Services { get; set; }
        public List<WorkingHourItem>? WorkingHours { get; set; }
        
        public string StaffInitial => string.IsNullOrEmpty(StaffName) ? "?" : StaffName[0].ToString();
    }

    public class WorkingHourItem
    {
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
    }

    public class StaffDetailViewModel
    {
        public Guid StaffId { get; set; }
        public Guid Id { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int CompletedAppointments { get; set; }
        public List<string> Services { get; set; } = new();
        public List<WorkingHourItem> WorkingHours { get; set; } = new();
        
        public string StaffInitial => string.IsNullOrEmpty(StaffName) ? "?" : StaffName[0].ToString();
    }
}
