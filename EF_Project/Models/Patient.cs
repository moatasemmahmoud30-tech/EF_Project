using EF_Project.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_Project.Models
{
    public class Patient : BaseEntity
    {
        [Key, ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public string BloodType { get; set; } = string.Empty;
        public string? Allergies { get; set; }
        public string? ChronicConditions { get; set; }

        public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
