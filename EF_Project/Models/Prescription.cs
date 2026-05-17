using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Project.Models
{
    public class Prescription : BaseEntity
    {
        [MaxLength(1000)]
        public string? GeneralNotes { get; set; }


        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        public virtual List<MedicalDetails> Items { get; set; } = new List<MedicalDetails>();
    }
}