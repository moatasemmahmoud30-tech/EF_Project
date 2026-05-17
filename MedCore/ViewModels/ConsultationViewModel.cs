using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedCore.ViewModels
{
    public class ConsultationViewModel
    {
        [Required]
        public int AppointmentId { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public string? Allergies { get; set; }
        public string? ChronicConditions { get; set; }

        public string? GeneralNotes { get; set; }

        public List<MedicalDetailsViewModel> PrescriptionItems { get; set; } = new List<MedicalDetailsViewModel>();
    }

}