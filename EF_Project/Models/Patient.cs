using EF_Project.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public class Patient : User
    {
        public BloodType BloodType { get; set; }

        public MedicalDetail Allergies { get; set; }
        public MedicalDetail ChronicConditions { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
