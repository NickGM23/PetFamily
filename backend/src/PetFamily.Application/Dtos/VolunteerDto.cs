
namespace PetFamily.Application.Dtos
{
    public class VolunteerDto
    {
        public Guid Id { get; init; }

        public string LastName { get; init; } = default!;

        public string FirstName { get; init; } = default!;

        public string? Patronymic { get; init; } = default!;

        public string Email { get; init; } = default!;

        public string Description { get; init; } = default!;

        public int YearsExperience { get; init; }

        public string PhoneNumber { get; init; } = default!;

        public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; } = [];

        public IEnumerable<RequisiteDto> Requisites { get; set; } = [];
    }
}
