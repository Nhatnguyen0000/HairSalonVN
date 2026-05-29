using System;

namespace HairSalonVN.API.Services.DTOs.Staff
{
    public class WorkingHourUpdateDto
    {
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
