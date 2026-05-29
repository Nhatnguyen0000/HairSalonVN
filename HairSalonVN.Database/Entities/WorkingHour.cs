using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonVN.Database.Entities
{
    public class WorkingHour
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StaffId { get; set; }
        public int DayOfWeek { get; set; }  // 0=Sun, 1=Mon … 6=Sat
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Staff Staff { get; set; } = null!;
    }

}
