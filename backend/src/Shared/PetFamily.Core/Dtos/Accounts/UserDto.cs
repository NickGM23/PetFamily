
namespace PetFamily.Core.Dtos.Accounts
{
    public class UserDto
    {
        public Guid Id { get; init; }
        public FullNameDto FullName { get; init; } = default!;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? PhoneNumber { get; init; } = string.Empty;
        public Guid RoleId { get; init; }
        public string PhotoPath { get; init; } = string.Empty;
        public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; } = [];
        public AdminAccountDto? AdminAccount { get; set; }
        public ParticipantAccountDto? ParticipantAccount { get; set; }
        public VolunteerAccountDto? VolunteerAccount { get; set; }
    }
}
