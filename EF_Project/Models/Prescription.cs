using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public class Prescription : BaseEntity
    {
        public string Dosage { get; set; }
        public string Frequency { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public int MedicationId { get; set; }
        public Medication Medication { get; set; }
    }
}
