using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Project.Data.Configurations
{
    public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasOne(s => s.Doctor)
                   .WithOne(d => d.Schedule)
                   .HasForeignKey<DoctorSchedule>(s => s.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}