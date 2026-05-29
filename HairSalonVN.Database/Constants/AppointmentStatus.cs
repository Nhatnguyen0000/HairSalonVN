using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonVN.Database.Constants
{
    public static class AppointmentStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All =
            { Pending, Confirmed, Completed, Cancelled };

        /// Trả true nếu có thể chuyển từ current → next
        public static bool CanTransition(string current, string next) =>
            (current, next) switch
            {
                (Pending, Confirmed) => true,
                (Pending, Cancelled) => true,
                (Confirmed, Completed) => true,
                (Confirmed, Cancelled) => true,
                _ => false,
            };
    }

}
