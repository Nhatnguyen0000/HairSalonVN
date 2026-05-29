using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalonVN.Database.Entities
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = "Cash"; // Cash|Card|Transfer
        public string Status { get; set; } = "Pending";
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        public Appointment Appointment { get; set; } = null!;
    }

}
