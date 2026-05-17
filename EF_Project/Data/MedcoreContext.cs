using EF_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EF_Project.Data
{
    public class MedCoreContext : IdentityDbContext<ApplicationUser>
    {
        public MedCoreContext(DbContextOptions<MedCoreContext> options) : base(options)
        {
        }

        public DbSet<Speciality> Specialties { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DailyAvailability> DailyAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MedicalDetails> PrescriptionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedCoreContext).Assembly);

            modelBuilder.Entity<Speciality>().HasData(
                new Speciality
                {
                    Id = 1,
                    Name = "Cardiology",
                    Image = "cardio-icon.png",
                    Description = "Heart and cardiovascular system care."
                },
                new Speciality
                {
                    Id = 2,
                    Name = "Neurology",
                    Image = "neuro-icon.png",
                    Description = "Brain and nervous system specialists."
                },
                new Speciality
                {
                    Id = 3,
                    Name = "Orthopedics",
                    Image = "ortho-icon.png",
                    Description = "Bones, joints, and musculoskeletal system."
                },
                new Speciality
                {
                    Id = 4,
                    Name = "Pediatrics",
                    Image = "peds-icon.png",
                    Description = "Comprehensive child medical care."
                }
            );

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) && entityType.BaseType == null)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("GETUTCDATE()");

                    ParameterExpression parameter = Expression.Parameter(entityType.ClrType, "e");
                    BinaryExpression body = Expression.Equal(
                        Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDeleted")),
                        Expression.Constant(false));

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyAuditRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditRules()
        {
            IEnumerable<EntityEntry<BaseEntity>> entries = ChangeTracker.Entries<BaseEntity>();
            foreach (EntityEntry<BaseEntity> entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }
        }
    }
}