using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_Project.Data.Configurations
{
    public class MedicalDetailConfiguration : IEntityTypeConfiguration<MedicalDetails>
    {
        public void Configure(EntityTypeBuilder<MedicalDetails> builder)
        {
            builder.HasOne(pi => pi.Prescription)
                   .WithMany(p => p.Items)
                   .HasForeignKey(pi => pi.PrescriptionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.Medication)
                   .WithMany(m => m.PrescriptionItems)
                   .HasForeignKey(pi => pi.MedicationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}