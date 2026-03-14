using EF_Project.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public class Appointment : BaseEntity
    {
        public AppointmentStatus Status { get; set; }
        public string CancellationReason { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int ScheduleId { get; set; }
        public DoctorSchedule Schedule { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
