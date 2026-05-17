using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Project.Data.Configurations
{
    public class DailyAvailabilityConfiguration : IEntityTypeConfiguration<DailyAvailability>
    {
        public void Configure(EntityTypeBuilder<DailyAvailability> builder)
        {
            builder.ToTable(t => t.HasCheckConstraint("CK_DailyAvailability_Time", "EndTime > StartTime"));

            builder.HasIndex(da => new { da.DoctorScheduleId, da.Day }).IsUnique();

            builder.HasOne(da => da.DoctorSchedule)
                   .WithMany(ds => ds.Availabilities)
                   .HasForeignKey(da => da.DoctorScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}