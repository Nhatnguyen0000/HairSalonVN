using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HairSalonVN.Database.Entities
{
    public class StaffService
    {
        public Guid StaffId { get; set; }
        public Guid ServiceId { get; set; }

        public Staff Staff { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }

}
