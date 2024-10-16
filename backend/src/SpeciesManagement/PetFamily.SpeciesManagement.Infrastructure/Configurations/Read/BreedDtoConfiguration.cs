
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dtos;

namespace PetFamily.SpeciesManagement.Infrastructure.Configurations.Read
{
    public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
    {
        public void Configure(EntityTypeBuilder<BreedDto> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SpeciesId);

            builder.Property(x => x.Name);

            builder.Property(x => x.Description);
        }
    }
}
