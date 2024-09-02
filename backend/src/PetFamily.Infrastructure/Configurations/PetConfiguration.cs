using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(id => id.Value,
                value => PetId.Create(value));

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            builder.ComplexProperty(p => p.Breed, pb =>
            {
                pb.Property(c => c.SpeciesId)
                   .IsRequired()
                   .HasConversion(
                   id => id.Value,
                   value => SpeciesId.Create(value)
                   )
                   .HasColumnName("species_id");
                pb.Property(c => c.BreedId)
                 .IsRequired()
                 .HasColumnName("breed_id");

            });

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

            builder.Property(p => p.Color)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            builder.Property(p => p.HealthInfo)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

            builder.ComplexProperty(p => p.Address, pa =>
            {
                pa.Property(a => a.Country)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                pa.Property(a => a.City)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                pa.Property(a => a.Street)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                pa.Property(a => a.PostalCode)
                  .IsRequired();

                pa.Property(a => a.HouseNumber)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                pa.Property(a => a.FlatNumber)
                   .IsRequired(false)
                   .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            });

            builder.Property(p => p.Weight)
                .IsRequired();

            builder.Property(p => p.Height)
                .IsRequired();

            builder.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT);

            builder.Property(p => p.IsCastrated)
                .IsRequired();

            builder.Property(p => p.BirthDay)
                .IsRequired()
                .HasMaxLength(Constants.MAX_DATE_LENGHT);

            builder.Property(p => p.IsVaccinated)
                .IsRequired();

            builder.Property(p => p.HelpStatus)
                .IsRequired()
                .HasConversion<string>();

            builder.OwnsOne(p => p.Requisites, pb =>
            {
                pb.ToJson();

                pb.OwnsMany(pr => pr.Requisites, prb =>
                {
                    prb.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                    prb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                });
            });

            builder.OwnsOne(p => p.PetPhotos, pb =>
            {
                pb.ToJson();

                pb.OwnsMany(pp => pp.PetPhotos, ppp =>
                {
                    ppp.Property(r => r.Path)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
                    ppp.Property(r => r.IsMain)
                    .IsRequired();
                });
            });

            builder.Property(p => p.DateCteate)
                .IsRequired();  
        }
    }
}
