namespace HairSalonVN.Web.Models.Booking
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public bool CanReview { get; set; }
    }

    public class CreateReviewViewModel
    {
        public Guid AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
