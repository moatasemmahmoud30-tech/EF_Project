using EF_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_Project.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.UseTptMappingStrategy();

            builder.HasIndex(u => u.NationalId).IsUnique();

            builder.ToTable(t => t.HasCheckConstraint("CK_User_DOB", "DateOfBirth < GETDATE()"));
        }
    }
}
