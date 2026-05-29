using System;

namespace HairSalonVN.API.Services.DTOs.Appointment
{
    public class AppointmentResponseDto
    {
        public Guid Id { get; set; }
        public string BookingCode { get; set; } = string.Empty;
        public Guid ServiceId { get; set; }
        public Guid StaffId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceImageUrl { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public int DurationMinutes { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsGuestBooking { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public bool HasReview { get; set; }
    }
}
