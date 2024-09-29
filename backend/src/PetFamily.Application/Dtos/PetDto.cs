using PetFamily.Domain.Enums;

namespace PetFamily.Application.Dtos
{
    public record PetDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = default!;

        public string Description { get; init; } = default!;

        public string Color { get; init; } = default!;

        public string HealthInfo { get; init; } = default!;

        public AddressDto Address { get; init; }

        public double Weight { get; init; }

        public double Height { get; init; }

        public string Phone { get; init; } = default!;

        public bool IsCastrated { get; init; }

        public DateOnly BirthDay { get; init; }

        public bool IsVaccinated { get; init; }

        public HelpStatus Status { get; init; }

        public IEnumerable<RequisiteDto> Requisites { get; init; }

        public Guid SpeciesId { get; init; }

        public Guid BreedId { get; init; }

        public PetDto()
        {
        }
    }
}
