using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Accounts;
using PetFamily.SharedKernel;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read
{
    public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
    {
        public void Configure(EntityTypeBuilder<UserDto> builder)
        {
            builder.ToTable("users");

            builder.HasKey(v => v.Id)
                .HasName("id");

            builder.Property(u => u.RoleId)
                .HasColumnName("role_id");

            builder.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number");

            builder.ComplexProperty(pa => pa.FullName, pab =>
            {
                pab.Property(u => u.FirstName)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("name");
                pab.Property(u => u.LastName)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("surname");
                pab.Property(u => u.Patronymic)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("patronymic");
            });

            builder.Property(u => u.UserName)
                .HasColumnName("user_name");

            builder.Property(p => p.SocialNetworks)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!);

            builder.HasOne(u => u.AdminAccount)
                .WithOne()
                .HasForeignKey<AdminAccountDto>(a => a.UserId);

            builder.HasOne(u => u.ParticipantAccount)
                .WithOne()
                .HasForeignKey<ParticipantAccountDto>(p => p.UserId);

            builder.HasOne(u => u.VolunteerAccount)
                .WithOne()
                .HasForeignKey<VolunteerAccountDto>(v => v.UserId);

        }
    }
}
