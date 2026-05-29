using System.ComponentModel.DataAnnotations;

namespace HairSalonVN.Web.Models.Booking
{
    public class AppointmentCreateViewModel
    {
        [Required] public Guid ServiceId { get; set; }
        [Required] public Guid StaffId { get; set; }
        [Required] public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
        // Dùng để hiển thị trên trang Confirm
        public string ServiceName { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public int Duration { get; set; }

        // Guest booking fields
        public bool IsGuestBooking { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public string? GuestEmail { get; set; }
    }

}
