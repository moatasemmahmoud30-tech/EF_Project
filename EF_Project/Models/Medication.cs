using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Project.Models
{
    public class Medication : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? GenericName { get; set; }

        public virtual List<MedicalDetails> PrescriptionItems { get; set; } = new List<MedicalDetails>();
    }
}