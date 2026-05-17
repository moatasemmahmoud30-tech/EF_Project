using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_Project.Models
{
    public class Doctor : BaseEntity
    {
        [Key, ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public string LicenseNumber { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string? Bio { get; set; }

        [Precision(18, 2)]
        public decimal HourlyRate { get; set; }

        public int SpecialityId { get; set; }
        public virtual Speciality Speciality { get; set; } = null!;

        public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual DoctorSchedule? Schedule { get; set; }
    }
}
