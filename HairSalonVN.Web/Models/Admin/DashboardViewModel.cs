using HairSalonVN.Web.Models.Booking;

namespace HairSalonVN.Web.Models.Admin;

public class DashboardViewModel
{
    public int TotalAppointmentsToday { get; set; }
    public int PendingCount { get; set; }
    public int CompletedCount { get; set; }
    public int TotalCount { get; set; }
    public decimal RevenueThisMonth { get; set; }
    public IEnumerable<AppointmentViewModel> RecentAppointments { get; set; } = [];
    public string RevenueDisplay => RevenueThisMonth > 0
        ? RevenueThisMonth.ToString("N0") + "đ"
        : "0đ";
}
