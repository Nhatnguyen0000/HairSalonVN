namespace HairSalonVN.Web.Models.Booking;

public class MyBookingsViewModel
{
    public List<AppointmentViewModel> Appointments { get; set; } = new();
    public int TotalCount { get; set; }
    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public int CompletedCount { get; set; }
    public int CancelledCount { get; set; }
}
