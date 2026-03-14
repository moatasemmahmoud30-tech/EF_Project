using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public class Medication : BaseEntity
    {
        public string Name { get; set; }
        public string GenericName { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
