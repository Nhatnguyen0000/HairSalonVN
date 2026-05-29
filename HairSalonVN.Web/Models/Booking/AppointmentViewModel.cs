namespace HairSalonVN.Web.Models.Booking
{
    public class AppointmentViewModel
    {
        public Guid Id { get; set; }
        public string BookingCode { get; set; } = string.Empty;
        public Guid ServiceId { get; set; }
        public Guid StaffId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceImageUrl { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Guest booking
        public bool IsGuestBooking { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }

        // Review status
        public bool HasReview { get; set; }

        // Helpers
        public string StatusClass => Status switch
        {
            "Pending" => "badge-pending",
            "Confirmed" => "badge-confirmed",
            "Completed" => "badge-completed",
            "Cancelled" => "badge-cancelled",
            _ => "",
        };

        public bool CanCancel => Status == "Pending" || Status == "Confirmed";
        public bool CanReview => Status == "Completed" && !HasReview;

        public string PriceDisplay => ServicePrice.ToString("N0") + "đ";

        public string StaffInitial => string.IsNullOrEmpty(StaffName) ? "?" : StaffName[0].ToString();
    }
}
