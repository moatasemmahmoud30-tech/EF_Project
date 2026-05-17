using EF_Project.Enums;
using Microsoft.AspNetCore.Identity;

namespace EF_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }


        public virtual Patient? PatientProfile { get; set; }
        public virtual Doctor? DoctorProfile { get; set; }
    }
}