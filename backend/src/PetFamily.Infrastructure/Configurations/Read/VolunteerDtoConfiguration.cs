
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using PetFamily.Core.Dtos;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerDto> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.LastName);

            builder.Property(v => v.FirstName);

            builder.Property(v => v.Patronymic);

            builder.Property(v => v.Email);

            builder.Property(v => v.Description);

            builder.Property(v => v.YearsExperience);

            builder.Property(v => v.PhoneNumber);

            builder.Property(i => i.SocialNetworks)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!);

            builder.Property(i => i.Requisites)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
