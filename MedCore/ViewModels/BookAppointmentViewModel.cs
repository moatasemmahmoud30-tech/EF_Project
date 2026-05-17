using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedCore.ViewModels
{
    public class BookAppointmentViewModel
    {
        [Required]
        public string DoctorId { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        [Required]
        public DateTime RequestedDate { get; set; }

        [Required]
        public TimeSpan SelectedTimeSlot { get; set; }

        public List<TimeSpan> AvailableTimeSlots { get; set; } = new List<TimeSpan>();
    }
}