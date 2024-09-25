using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersManagement.Entities;

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

            builder.ComplexProperty(p => p.Name, pb =>
            {
                pb.Property(pp => pp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("name");
            });

            builder.ComplexProperty(p => p.Breed, pb =>
            {
                pb.Property(c => c.SpeciesId)
                   .IsRequired()
                   .HasConversion(id => id.Value,
                   value => SpeciesId.Create(value))
                   .HasColumnName("species_id");
                pb.Property(c => c.BreedId)
                 .IsRequired()
                 .HasColumnName("breed_id");
            });

            builder.ComplexProperty(p => p.Description, pb =>
            {
                pb.Property(pp => pp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .HasColumnName("description");
            });

            builder.ComplexProperty(p => p.Color, pb =>
            {
                pb.Property(pp => pp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("color");
            });

            builder.ComplexProperty(p => p.HealthInfo, pb =>
            {
                pb.Property(pp => pp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .HasColumnName("health_info");
            });

            builder.ComplexProperty(p => p.Address, pa =>
            {
                pa.Property(a => a.Country)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                  .HasColumnName("country");

                pa.Property(a => a.City)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                  .HasColumnName("city"); 

                pa.Property(a => a.Street)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                  .HasColumnName("street");

                pa.Property(a => a.PostalCode)
                  .IsRequired()
                  .HasColumnName("postal_code");

                pa.Property(a => a.HouseNumber)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                   .HasColumnName("house_number");

                pa.Property(a => a.FlatNumber)
                   .IsRequired(false)
                   .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                   .HasColumnName("flat_number");
            });

            builder.Property(p => p.Weight)
                .IsRequired();

            builder.Property(p => p.Height)
                .IsRequired();

            builder.ComplexProperty(v => v.PhoneNumber, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT)
                    .HasColumnName("phone_number");
            });

            builder.Property(p => p.IsCastrated)
                .IsRequired();

            builder.Property(p => p.BirthDay)
                .IsRequired()
                .HasMaxLength(Constants.MAX_DATE_LENGHT);

            builder.Property(p => p.IsVaccinated)
                .IsRequired();

            builder.Property(p => p.HelpStatus)
                .HasConversion(
                status => status.ToString(),
                value => (HelpStatus)Enum.Parse(typeof(HelpStatus), value));

            builder.OwnsOne(p => p.Requisites, pb =>
            {
                pb.ToJson("requisites");

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
                pb.ToJson("photos");

                pb.OwnsMany(pp => pp.PetPhotos, ppp =>
                {
                    ppp.Property(r => r.Path)
                       .HasConversion(
                            p => p.Path,
                            value => FilePath.Create(value).Value)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                    ppp.Property(r => r.IsMain)
                        .IsRequired();
                });
            });

            builder.Property(p => p.DateCteate)
                .IsRequired();

            builder.Property<bool>("_isDeleted")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("is_deleted");

            builder.ComplexProperty(v => v.SerialNumber, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasColumnName("serial_number");
            });
        }
    }
}
