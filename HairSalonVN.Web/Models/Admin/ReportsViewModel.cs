namespace HairSalonVN.Web.Models.Admin;

public class ReportsViewModel
{
    public decimal RevenueThisMonth { get; set; }
    public decimal RevenueAllTime { get; set; }
    public int AppointmentsThisMonth { get; set; }
    public int CompletedThisMonth { get; set; }
    public int CancelledThisMonth { get; set; }
    public int TotalCustomers { get; set; }
    public IEnumerable<ServiceStatItem> TopServices { get; set; } = [];
    public IEnumerable<StaffStatItem> TopStaff { get; set; } = [];
}

public class ServiceStatItem
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Revenue { get; set; }
}

public class StaffStatItem
{
    public string Name { get; set; } = string.Empty;
    public int CompletedCount { get; set; }
}
