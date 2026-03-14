using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EF_Project.Data;
using EF_Project.Enums;
using EF_Project.Models;

Console.WriteLine("MedCore System Initializing...\n");

using var context = new MedCoreContext();


Console.WriteLine("Step 1: Verifying Database and Seeding Data...");

if (!context.Doctors.Any())
{
    var cardiology = new Specialty
    {
        Name = "Cardiology",
        Image = "...",
        Description = "Heart and cardiovascular system"
    };

    var doctor = new Doctor
    {
        FullName = "Moatasem Mahmoud",
        NationalId = "12345678901234", 
        DateOfBirth = new DateTime(1980, 5, 12),
        Gender = Gender.Female,
        LicenseNumber = "MED-998877",
        HireDate = DateTime.UtcNow,
        HourRate = 150.00m,
        Specialty = cardiology,
        ProfileImage = "..."
    };

    var patient = new Patient
    {
        FullName = "Ramadan Abbas",
        NationalId = "98765432109876",
        DateOfBirth = new DateTime(1975, 3, 15),
        Gender = Gender.Male,
        BloodType = BloodType.OPositive,
        Allergies = new MedicalDetail { Description = "Penicillin" },
        ChronicConditions = new MedicalDetail { Description = "None" },
        ProfileImage = "..."
    };

    context.Specialties.Add(cardiology);
    context.Doctors.Add(doctor);
    context.Patients.Add(patient);

    context.SaveChanges();
    Console.WriteLine("Data successfully saved!\n");
}

Console.WriteLine("Step 2: Retrieving Data...");

var savedDoc = context.Doctors
    .Include(d => d.Specialty)
    .FirstOrDefault();

var savedPatient = context.Patients.FirstOrDefault();

if (savedDoc != null && savedPatient != null)
{
    Console.WriteLine($"Doctor: {savedDoc.FullName} | Specialty: {savedDoc.Specialty.Name}");

    var createdAt = context.Entry(savedDoc).Property("CreatedAt").CurrentValue;
    Console.WriteLine($"Doctor Created At (Shadow Property): {createdAt}");

    Console.WriteLine($"\nPatient: {savedPatient.FullName} | Blood Type: {savedPatient.BloodType}");
    Console.WriteLine($"Patient Allergies: {savedPatient.Allergies.Description}");
}

Console.WriteLine("\nPress any key to exit...");