using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedCore.ViewModels
{
    public class ConsultationSubmissionViewModel
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public string PatientId { get; set; } = string.Empty;

        [Required]
        public string Diagnosis { get; set; } = string.Empty;

        public List<PrescriptionItemViewModel> Items { get; set; } = new List<PrescriptionItemViewModel>();
    }

    public class PrescriptionItemViewModel
    {
        public int MedicationId { get; set; }
        public string Dosage { get; set; } = string.Empty;
    }
}