using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Project.Data.Configurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasOne(p => p.Appointment)
                   .WithMany(a => a.Prescriptions)
                   .HasForeignKey(p => p.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}