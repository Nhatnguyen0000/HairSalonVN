using HairSalonVN.Database.Entities;

namespace HairSalonVN.API.Helpers;

public static class WorkingHoursHelper
{
    public static (TimeSpan Start, TimeSpan End) Resolve(
        WorkingHour? wh, int dayOfWeek)
    {
        if (wh is not null)
            return (wh.StartTime, wh.EndTime);

        // No fallback — staff doesn't work on days without a WorkingHour record.
        // Return invalid range so callers treat this as "unavailable".
        return (TimeSpan.Zero, TimeSpan.Zero);
    }
}
