﻿
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
    {
        public void Configure(EntityTypeBuilder<SpeciesDto> builder)
        {
            builder.ToTable("species");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name);

            builder.Property(x => x.Description);
        }
    }
}
