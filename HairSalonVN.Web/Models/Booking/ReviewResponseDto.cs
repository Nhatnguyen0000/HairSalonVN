namespace HairSalonVN.Web.Models.Booking
{
    public class ReviewResponseDto
    {
        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;
    }
}