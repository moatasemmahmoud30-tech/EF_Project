using EF_Project.Data;
using EF_Project.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCore.Services
{
    public class SchedulingService : ISchedulingService
    {
        private readonly MedCoreContext _context;

        public SchedulingService(MedCoreContext context)
        {
            _context = context;
        }

        public async Task<List<TimeSpan>> GetAvailableTimeSlotsAsync(string doctorId, DateTime requestedDate)
        {
            DayOfWeek dayOfWeek = requestedDate.DayOfWeek;

            var doctorSchedule = await _context.DoctorSchedules
                .Include(ds => ds.Availabilities)
                .FirstOrDefaultAsync(ds => ds.DoctorId == doctorId);

            if (doctorSchedule == null)
                return new List<TimeSpan>();

            var dailyTemplate = doctorSchedule.Availabilities
                .FirstOrDefault(a => a.Day == dayOfWeek && a.IsWorking);

            if (dailyTemplate == null)
                return new List<TimeSpan>(); 

            List<TimeSpan> bookedTimes = await _context.Appointments
                .Where(a => a.DoctorId == doctorId
                         && a.AppointmentDate.Date == requestedDate.Date
                         && a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.AppointmentDate.TimeOfDay)
                .ToListAsync();

            List<TimeSpan> availableSlots = new List<TimeSpan>();
            TimeSpan currentSlotStart = dailyTemplate.StartTime;
            TimeSpan slotDuration = TimeSpan.FromMinutes(doctorSchedule.SlotDurationMinutes);

            while (currentSlotStart.Add(slotDuration) <= dailyTemplate.EndTime)
            {
                if (!bookedTimes.Contains(currentSlotStart))
                {
                    availableSlots.Add(currentSlotStart);
                }

                currentSlotStart = currentSlotStart.Add(slotDuration);
            }

            return availableSlots;
        }
    }
}