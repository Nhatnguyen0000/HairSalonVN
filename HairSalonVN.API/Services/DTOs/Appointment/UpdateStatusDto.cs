using System;

namespace HairSalonVN.API.Services.DTOs.Appointment
{
    public class UpdateStatusDto
    {
        public string? Status { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Status)
            && (Status == "Confirmed" || Status == "Completed" || Status == "Cancelled" || Status == "Pending");
    }
}
