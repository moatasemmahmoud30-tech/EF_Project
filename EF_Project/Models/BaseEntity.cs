using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public byte[] Version { get; set; }

        public DateTime LastModified { get; set; }
        public bool IsDeleted { get; set; }

    }
}
