namespace HairSalonVN.Web.Models.Booking
{
    public class AvailableSlotViewModel
    {
        public DateTime Time { get; set; }
        public string Label { get; set; } = string.Empty;
        public bool IsAvail { get; set; }
        
        // Alias for compatibility
        public bool IsAvailable => IsAvail;
        public bool isAvail => IsAvail;
        public bool isAvailable => IsAvail;
    }
}
