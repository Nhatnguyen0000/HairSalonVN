using HairSalonVN.Web.Models.Booking;

namespace HairSalonVN.Web.Models.Staff;

public class StaffDashboardViewModel
{
    public List<AppointmentViewModel> TodayAppointments { get; set; } = new();
    public List<AppointmentViewModel> UpcomingAppointments { get; set; } = new();
    public List<AppointmentViewModel> AllAppointments { get; set; } = new();
    public int CompletedToday { get; set; }
    public int PendingCount { get; set; }
    public int TotalToday { get; set; }
    public string UserName { get; set; } = string.Empty;
}

public class StaffMyBookingsViewModel
{
    public List<AppointmentViewModel> AllAppointments { get; set; } = new();
    public int CompletedCount { get; set; }
    public int CancelledCount { get; set; }
    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public decimal Revenue => AllAppointments
        .Where(a => a.Status == "Completed")
        .Sum(a => a.ServicePrice);
    public string RevenueDisplay => Revenue > 0 ? Revenue.ToString("N0") + "đ" : "0đ";
    public string UserName { get; set; } = string.Empty;
}
