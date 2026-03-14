using EF_Project.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public abstract class User : BaseEntity
    {
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string ProfileImage { get; set; }
    }
}
