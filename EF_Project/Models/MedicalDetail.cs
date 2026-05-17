using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Project.Models
{
    public class MedicalDetails : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public int MedicationId { get; set; }
        public virtual Medication Medication { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Frequency { get; set; } = string.Empty;

        public string? AdditionalInstructions { get; set; }

        public int PrescriptionId { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; } = null!;
    }
}