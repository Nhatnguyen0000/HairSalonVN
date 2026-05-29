using System;
using System.ComponentModel.DataAnnotations;

namespace HairSalonVN.API.Services.DTOs.Appointment
{
    public class AppointmentCreateDto
    {
        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public Guid StaffId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public bool IsGuest { get; set; }

        [StringLength(100)]
        public string? GuestName { get; set; }

        [Phone]
        public string? GuestPhone { get; set; }

        [EmailAddress]
        public string? GuestEmail { get; set; }
    }
}
