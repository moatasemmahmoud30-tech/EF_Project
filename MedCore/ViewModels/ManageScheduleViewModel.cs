using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedCore.ViewModels
{
    public class DailyScheduleViewModel
    {
        public DayOfWeek Day { get; set; }

        public bool IsWorking { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
    }

    public class ManageScheduleViewModel
    {
        public List<DailyScheduleViewModel> Days { get; set; } = new List<DailyScheduleViewModel>();

        [Required]
        [Display(Name = "Consultation Duration (Minutes)")]
        [Range(15, 120, ErrorMessage = "Slot duration must be between 15 and 120 minutes.")]
        public int SlotDurationMinutes { get; set; } = 30; 
    }
}