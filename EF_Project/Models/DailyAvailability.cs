using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Project.Models
{
    public class DailyAvailability : BaseEntity
    {
        [ForeignKey("DoctorSchedule")]
        public int DoctorScheduleId { get; set; }
        public virtual DoctorSchedule DoctorSchedule { get; set; } = null!;

        public DayOfWeek Day { get; set; }
        public bool IsWorking { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}