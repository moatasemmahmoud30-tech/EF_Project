using EF_Project.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Project.Models
{
    public class Appointment : BaseEntity
    {
        public AppointmentStatus Status { get; set; }
        public string? CancellationReason { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string PatientId { get; set; } = string.Empty;
        public Patient Patient { get; set; } = null!;

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!;
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}