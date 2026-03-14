using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Data.Configurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasIndex(p => new { p.AppointmentId, p.MedicationId }).IsUnique();

            builder.HasOne(p => p.Appointment)
                   .WithMany(a => a.Prescriptions)
                   .HasForeignKey(p => p.AppointmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Medication)
                   .WithMany(m => m.Prescriptions)
                   .HasForeignKey(p => p.MedicationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
