namespace HairSalonVN.Web.Models.Admin
{
    public class StaffHoursEditViewModel
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public List<DayHourRow> Days { get; set; } = new();
    }

    public class DayHourRow
    {
        public int DayOfWeek { get; set; }
        public string DayLabel { get; set; } = string.Empty;
        public string StartTime { get; set; } = "08:00";
        public string EndTime { get; set; } = "20:00";
        public bool IsOpen { get; set; } = true;
    }
}
