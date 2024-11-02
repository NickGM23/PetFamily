using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dtos.Accounts;
using PetFamily.Core.Dtos;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read
{
    public class VolunteerAccountDtoConfiguration : IEntityTypeConfiguration<VolunteerAccountDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
        {
            builder.ToTable("volunteers");
            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(v => v.Experience)
                .HasColumnName("experience");

            builder.Property(v => v.Requisites)
                 .HasConversion(
                    requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);

            builder.Property(v => v.UserId)
                .HasColumnName("user_id");
        }
    }
}
