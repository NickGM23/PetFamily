using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.SpeciesManagement.Domain.Entities;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasConversion(id => id.Value,
                value => BreedId.Create(value));

            builder.ComplexProperty(v => v.Name, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("name");
            });

            builder.ComplexProperty(v => v.Description, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .HasColumnName("description");
            });

            builder.Property<bool>("_isDeleted")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("is_deleted");
        }
    }
}
