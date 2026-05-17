using System;
using System.ComponentModel.DataAnnotations;

namespace EF_Project.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}