using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EF_Project.Models
{
    public class Specialty : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
