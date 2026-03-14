using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(a => a.Schedule)
                   .WithOne(s => s.Appointment)
                   .HasForeignKey<Appointment>(a => a.ScheduleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
