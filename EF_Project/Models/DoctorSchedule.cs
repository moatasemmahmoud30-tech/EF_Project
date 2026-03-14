using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public class DoctorSchedule : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public Appointment Appointment { get; set; }
    }
}
