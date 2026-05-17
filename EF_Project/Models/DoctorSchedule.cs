using System.Collections.Generic;

namespace EF_Project.Models
{
    public class DoctorSchedule : BaseEntity
    {

        public int SlotDurationMinutes { get; set; } = 30;

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!;

        public virtual List<DailyAvailability> Availabilities { get; set; } = new List<DailyAvailability>();
    }
}