using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dtos.Accounts;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read
{
    public class ParticipantAccountDtoConfiguration : IEntityTypeConfiguration<ParticipantAccountDto>
    {
        public void Configure(EntityTypeBuilder<ParticipantAccountDto> builder)
        {
            builder.ToTable("participant_accounts");
            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(v => v.UserId)
                .HasColumnName("user_id");
        }
    }
}
