using System;

namespace HairSalonVN.API.Services.DTOs.Appointment
{
    public class AvailableSlotDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }

        public string StartDisplay => $"{(int)StartTime.TotalHours:D2}:{StartTime.Minutes:D2}";
        public string EndDisplay => $"{(int)EndTime.TotalHours:D2}:{EndTime.Minutes:D2}";
    }
}
