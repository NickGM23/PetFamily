using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Core.Extensions;
using PetFamily.Core.Dtos;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.OwnsOne(u => u.SocialNetworks, sb =>
            {
                sb.Property(s => s.SocialNetworks)
                    .ValueObjectsCollectionJsonConversion(
                        sn => new SocialNetworkDto(sn.Link, sn.Name),
                        dto => SocialNetwork.Create(dto.Link, dto.Name).Value)
                    .HasColumnName("social_networks");
            });

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ComplexProperty(u => u.FullName, nameBuilder =>
            {
                nameBuilder.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("first_name");
                nameBuilder.Property(fn => fn.Patronymic)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("patronymic");
                nameBuilder.Property(fn => fn.LastName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("last_name"); ;
            });
        }
    }
}
