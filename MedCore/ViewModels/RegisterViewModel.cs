using System;
using System.ComponentModel.DataAnnotations;

namespace MedCore.ViewModels
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "National ID is required")]
        [Display(Name = "National ID")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Role { get; set; } = "Patient";


        [Display(Name = "Blood Type")]
        public string? BloodType { get; set; }

        public string? Allergies { get; set; }

        [Display(Name = "Chronic Conditions")]
        public string? ChronicConditions { get; set; }


        [Display(Name = "License Number")]
        public string? LicenseNumber { get; set; }

        [Display(Name = "Years of Experience")]
        [Range(0, 100, ErrorMessage = "Please enter a valid number of years")]
        public int? YearsOfExperience { get; set; }

        [Display(Name = "Speciality")]
        public int? SpecialityId { get; set; }

        [Display(Name = "Hourly Rate ($)")]
        [DataType(DataType.Currency)]
        public decimal HourlyRate { get; set; }
    }
}