using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.SpeciesManagement;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasConversion(id => id.Value,
                value => SpeciesId.Create(value));

            builder.ComplexProperty(p => p.Name, pb =>
            {
                pb.Property(pp => pp.Value)
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

            builder.HasMany(v => v.Breeds)
                .WithOne()
                .HasForeignKey("species_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Navigation(v => v.Breeds).AutoInclude();

            builder.Property<bool>("_isDeleted")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("is_deleted");
        }
    }
}
