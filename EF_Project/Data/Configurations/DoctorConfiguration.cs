using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Project.Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(d => d.Speciality)
                   .WithMany(s => s.Doctors) 
                   .HasForeignKey(d => d.SpecialityId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Schedule)
                   .WithOne(s => s.Doctor)
                   .HasForeignKey<DoctorSchedule>(s => s.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}