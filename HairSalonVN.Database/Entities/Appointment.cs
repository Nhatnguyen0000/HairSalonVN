using HairSalonVN.Database.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonVN.Database.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? CustomerId { get; set; }
        public Guid StaffId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = AppointmentStatus.Pending;
        public string BookingCode { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ── Guest Booking Fields ──────────────────────────────────
        public bool IsGuestBooking { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public string? GuestEmail { get; set; }

        // ── Navigation ────────────────────────────────────────────
        public User? Customer { get; set; }
        public Staff Staff { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
    }

}
