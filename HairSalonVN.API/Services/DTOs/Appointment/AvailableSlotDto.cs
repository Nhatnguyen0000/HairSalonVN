using System;

namespace HairSalonVN.API.Services.DTOs.Appointment
{
    public class AvailableSlotDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        
        // Alias for JS compatibility (camelCase)
        public bool isAvail => IsAvailable;
        public bool IsAvail => IsAvailable;
        public bool isAvailable => IsAvailable;
        
        // Label for display
        public string Label => $"{(int)StartTime.TotalHours:D2}:{StartTime.Minutes:D2}";
        public string label => Label;
        public string Time => Label;
        public string time => Label;
        
        public string StartDisplay => $"{(int)StartTime.TotalHours:D2}:{StartTime.Minutes:D2}";
        public string EndDisplay => $"{(int)EndTime.TotalHours:D2}:{EndTime.Minutes:D2}";
    }
}
