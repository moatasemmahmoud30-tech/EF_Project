using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedCore.Services
{
    public interface ISchedulingService
    {
        Task<List<TimeSpan>> GetAvailableTimeSlotsAsync(string doctorId, DateTime requestedDate);
    }
}