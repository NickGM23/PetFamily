﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");

            builder.HasIndex(p => p.Code)
                .IsUnique();
        }
    }
}