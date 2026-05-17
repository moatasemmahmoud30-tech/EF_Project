using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedCore.ViewModels
{
    public class MedicalDetailsViewModel
    {
        [Required]
        public int MedicationId { get; set; }

        [Required]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        public string Frequency { get; set; } = string.Empty;
    }

}