
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dtos;
using System.Text.Json;

namespace PetFamily.VolunteerManagement.Infrastructure.Configurations.Read
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name);

            builder.Property(p => p.Description);

            builder.Property(p => p.Color);

            builder.Property(p => p.HealthInfo)
                .HasColumnName("health_info");

            builder.ComplexProperty(p => p.Address, pa =>
            {
                pa.Property(a => a.Country).
                    HasColumnName("country");

                pa.Property(a => a.City).
                    HasColumnName("city");

                pa.Property(a => a.Street).
                    HasColumnName("street");

                pa.Property(a => a.PostalCode).
                    HasColumnName("postal_code");

                pa.Property(a => a.HouseNumber).
                    HasColumnName("house_number");

                pa.Property(a => a.FlatNumber)
                    .HasColumnName("flat_number");
            });

            builder.Property(p => p.Weight);

            builder.Property(p => p.Height);

            builder.Property(b => b.Phone)
                    .HasColumnName("phone_number");

            builder.Property(p => p.IsCastrated);

            builder.Property(p => p.BirthDay).
                HasColumnName("birth_day");

            builder.Property(p => p.IsVaccinated);

            builder.Property(p => p.Status)
                .HasColumnName("help_status")
                .HasColumnType("varchar");

            builder.Property(p => p.SpeciesId);

            builder.Property(p => p.BreedId);

            builder.Property(i => i.Requisites)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);

            builder.Property(i => i.Photos)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<PetPhotoDto[]>(json, JsonSerializerOptions.Default)!);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.IsDeleted)
                .HasColumnName("is_deleted");
        }
    }
}
