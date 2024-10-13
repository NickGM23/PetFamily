using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Entities;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.VolunteerManagement.Infrastructure.Extensions;

namespace PetFamily.VolunteerManagement.Infrastructure.Configurations.Write
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

            builder.OwnsOne(v => v.Requisites, vb =>
            {
                vb.Property(r => r.Requisites)
                    .ValueObjectsCollectionJsonConversion(
                        r => new RequisiteDto(r.Name, r.Description),
                        dto => Requisite.Create(dto.Name, dto.Description).Value)
                    .HasColumnName("requisites");
            });

            builder.OwnsOne(v => v.PetPhotos, vb =>
            {
                vb.Property(r => r.PetPhotos)
                    .ValueObjectsCollectionJsonConversion(
                        r => new PetPhotoDto(r.Path.Path, r.IsMain),
                        dto => PetPhoto.Create(FilePath.Create(dto.Path).Value, dto.IsMain).Value)
                    .HasColumnName("photos");
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
