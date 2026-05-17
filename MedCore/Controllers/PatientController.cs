using EF_Project.Data;
using EF_Project.Models;
using EF_Project.Enums;
using MedCore.Services;
using MedCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedCore.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly MedCoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchedulingService _schedulingService;

        public PatientController(
            MedCoreContext context,
            UserManager<ApplicationUser> userManager,
            ISchedulingService schedulingService)
        {
            _context = context;
            _userManager = userManager;
            _schedulingService = schedulingService;
        }

        [HttpGet]
        public async Task<IActionResult> BookAppointment(string doctorId, DateTime date)
        {
            if (date.Date < DateTime.UtcNow.Date)
            {
                return BadRequest("Cannot book appointments in the past.");
            }

            Doctor doctorEntity = await _context.Doctors
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.ApplicationUserId == doctorId);

            if (doctorEntity == null) return NotFound("Doctor not found.");

            List<TimeSpan> availableSlots = await _schedulingService.GetAvailableTimeSlotsAsync(doctorId, date);

            BookAppointmentViewModel model = new BookAppointmentViewModel
            {
                DoctorId = doctorEntity.ApplicationUserId,
                DoctorName = doctorEntity.ApplicationUser.FullName,
                RequestedDate = date,
                AvailableTimeSlots = availableSlots
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(BookAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableTimeSlots = await _schedulingService.GetAvailableTimeSlotsAsync(model.DoctorId, model.RequestedDate);
                return View("BookAppointment", model);
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            Patient patientEntity = await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

            List<TimeSpan> currentAvailableSlots = await _schedulingService.GetAvailableTimeSlotsAsync(model.DoctorId, model.RequestedDate);

            if (!currentAvailableSlots.Contains(model.SelectedTimeSlot))
            {
                ModelState.AddModelError(string.Empty, "Oops — Someone Beat You to It! That slot is no longer available.");
                model.AvailableTimeSlots = currentAvailableSlots;
                return View("BookAppointment", model);
            }

            Appointment newAppointment = new Appointment
            {
                DoctorId = model.DoctorId,
                PatientId = patientEntity.ApplicationUserId,
                AppointmentDate = model.RequestedDate.Date.Add(model.SelectedTimeSlot),
                Status = AppointmentStatus.Scheduled
            };

            _context.Appointments.Add(newAppointment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(string.Empty, "Oops — Someone Beat You to It! The slot was just booked by another patient.");
                model.AvailableTimeSlots = await _schedulingService.GetAvailableTimeSlotsAsync(model.DoctorId, model.RequestedDate);
                return View("BookAppointment", model);
            }

            return RedirectToAction("MedicalRecords");
        }

        [HttpGet]
        public async Task<IActionResult> FindDoctor(int? specialtyId)
        {
            var doctorsQuery = _context.Doctors
                .Include(d => d.ApplicationUser)
                .Include(d => d.Speciality)
                .AsQueryable();

            if (specialtyId.HasValue)
            {
                doctorsQuery = doctorsQuery.Where(d => d.SpecialityId == specialtyId.Value);
            }

            ViewBag.Specialties = await _context.Specialties.ToListAsync();

            return View(await doctorsQuery.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MedicalRecords()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.ApplicationUser)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Speciality)
                .Include(a => a.Prescriptions) 
                    .ThenInclude(p => p.Items)
                .Where(a => a.PatientId == user.Id) 
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }
    }
}