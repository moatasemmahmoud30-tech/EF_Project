using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EF_Project.Models
{
    public class Speciality : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
