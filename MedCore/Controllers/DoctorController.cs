using EF_Project.Data;
using EF_Project.Enums;
using EF_Project.Models;
using MedCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCore.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly MedCoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(MedCoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Schedule()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found.");

            Doctor doctor = await _context.Doctors
                .Include(d => d.Schedule)
                    .ThenInclude(s => s.Availabilities)
                .FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);

            if (doctor == null) return NotFound("Doctor profile not found.");

            ManageScheduleViewModel model = new ManageScheduleViewModel();
            model.Days = new List<DailyScheduleViewModel>();

            DayOfWeek[] allDays = (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek));

            foreach (DayOfWeek day in allDays)
            {
                DailyAvailability existingDay = doctor.Schedule?.Availabilities.FirstOrDefault(a => a.Day == day);

                if (existingDay != null)
                {
                    model.Days.Add(new DailyScheduleViewModel
                    {
                        Day = day,
                        IsWorking = existingDay.IsWorking,
                        StartTime = existingDay.StartTime,
                        EndTime = existingDay.EndTime
                    });
                }
                else
                {
                    bool isWeekend = (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday);
                    model.Days.Add(new DailyScheduleViewModel
                    {
                        Day = day,
                        IsWorking = !isWeekend,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0)
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(ManageScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found.");

            Doctor doctorEntity = await _context.Doctors
                .Include(d => d.Schedule)
                    .ThenInclude(s => s.Availabilities)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);

            if (doctorEntity == null) return NotFound("Doctor profile not found.");

            if (doctorEntity.Schedule == null)
            {
                doctorEntity.Schedule = new DoctorSchedule
                {
                    DoctorId = doctorEntity.ApplicationUserId,
                    Availabilities = new List<DailyAvailability>()
                };
                _context.DoctorSchedules.Add(doctorEntity.Schedule);
            }

            doctorEntity.Schedule.SlotDurationMinutes = model.SlotDurationMinutes;

            foreach (DailyScheduleViewModel dayModel in model.Days)
            {
                DailyAvailability existingDay = doctorEntity.Schedule.Availabilities
                    .FirstOrDefault(a => a.Day == dayModel.Day);

                if (dayModel.IsWorking)
                {
                    if (existingDay != null)
                    {
                        existingDay.IsDeleted = false; 
                        existingDay.IsWorking = true;
                        existingDay.StartTime = dayModel.StartTime;
                        existingDay.EndTime = dayModel.EndTime;
                    }
                    else
                    {
                        doctorEntity.Schedule.Availabilities.Add(new DailyAvailability
                        {
                            DoctorScheduleId = doctorEntity.Schedule.Id,
                            Day = dayModel.Day,
                            IsWorking = true,
                            StartTime = dayModel.StartTime,
                            EndTime = dayModel.EndTime
                        });
                    }
                }
                else
                {
                    if (existingDay != null && !existingDay.IsDeleted)
                    {
                        _context.DailyAvailabilities.Remove(existingDay);
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Schedule successfully updated.";
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> ConsultationRoom(int appointmentId)
        {
            Appointment appointment = await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null) return NotFound("Appointment not found.");

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (appointment.DoctorId != user.Id) return Unauthorized();

            ConsultationViewModel model = new ConsultationViewModel
            {
                AppointmentId = appointment.Id,
                PatientName = appointment.Patient.ApplicationUser.FullName,
                BloodType = appointment.Patient.BloodType,
                Allergies = appointment.Patient.Allergies,
                ChronicConditions = appointment.Patient.ChronicConditions
            };

            ViewBag.Medications = await _context.Medications.ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPrescription(ConsultationViewModel model)
        {
            if (!ModelState.IsValid || model.PrescriptionItems.Count == 0)
            {
                ViewBag.Medications = await _context.Medications.ToListAsync();
                return View("ConsultationRoom", model);
            }

            Prescription newPrescription = new Prescription
            {
                AppointmentId = model.AppointmentId,
                GeneralNotes = model.GeneralNotes,
                Items = new List<MedicalDetails>()
            };

            foreach (MedicalDetailsViewModel item in model.PrescriptionItems)
            {
                newPrescription.Items.Add(new MedicalDetails
                {
                    MedicationId = item.MedicationId,
                    Dosage = item.Dosage,
                    Frequency = item.Frequency
                });
            }

            Appointment appointment = await _context.Appointments.FindAsync(model.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Completed;
            }

            _context.Prescriptions.Add(newPrescription);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Prescription saved and appointment completed.";
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> Consultation(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == id && a.DoctorId == user.Id);

            if (appointment == null) return NotFound("Appointment not found.");

            var viewModel = new ConsultationViewModel
            {
                AppointmentId = appointment.Id,
                PatientName = appointment.Patient?.ApplicationUser?.FullName ?? "Unknown",
                BloodType = appointment.Patient?.BloodType ?? "Not Recorded",
                Allergies = appointment.Patient?.Allergies ?? "None",
                ChronicConditions = appointment.Patient?.ChronicConditions ?? "None",
                GeneralNotes = string.Empty
            };

            ViewBag.AppointmentDate = appointment.AppointmentDate;
            ViewBag.Medications = await _context.Medications.OrderBy(m => m.Name).ToListAsync();

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteConsultation(ConsultationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid consultation data.");

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == model.AppointmentId && a.DoctorId == user.Id);

            if (appointment == null)
                return NotFound("Appointment not found or unauthorized.");

            Prescription newPrescription = new Prescription
            {
                AppointmentId = model.AppointmentId,
                GeneralNotes = model.GeneralNotes, 
                Items = new List<MedicalDetails>()
            };

            if (model.PrescriptionItems != null && model.PrescriptionItems.Count > 0)
            {
                foreach (var item in model.PrescriptionItems)
                {
                    newPrescription.Items.Add(new MedicalDetails
                    {
                        MedicationId = item.MedicationId,
                        Dosage = item.Dosage,
                        Frequency = item.Frequency
                    });
                }
            }

            _context.Prescriptions.Add(newPrescription);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Consultation successfully completed and prescription saved.";
            return RedirectToAction(nameof(Dashboard));
        }
    }
}