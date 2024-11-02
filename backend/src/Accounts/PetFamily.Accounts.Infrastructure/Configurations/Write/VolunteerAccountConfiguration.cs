
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.TypeAccounts;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write
{
    public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
    {
        public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
        {
            builder.ToTable("volunteer_accounts");

            builder.HasKey(b => b.Id);

            builder.OwnsOne(v => v.Requisites, vb =>
            {
                vb.Property(r => r.Requisites)
                    .ValueObjectsCollectionJsonConversion(
                        r => new RequisiteDto(r.Name, r.Description),
                        dto => Requisite.Create(dto.Name, dto.Description).Value)
                    .HasColumnName("requisites");
            });
        }
    }
}
