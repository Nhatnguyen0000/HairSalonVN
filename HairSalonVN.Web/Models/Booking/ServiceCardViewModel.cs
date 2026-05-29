namespace HairSalonVN.Web.Models.Booking
{
    public class ServiceCardViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
        // Hiển thị friendly
        public string PriceDisplay => Price.ToString("N0") + "đ";
        public string DurationDisplay => DurationMinutes >= 60
            ? $"{DurationMinutes / 60}h{DurationMinutes % 60:00}p"
            : $"{DurationMinutes} phút";
    }

}
