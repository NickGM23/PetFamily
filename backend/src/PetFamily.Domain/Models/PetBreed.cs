
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Models
{
    public record PetBreed
    {
        public SpeciesId SpeciesId { get; } = null!;

        public Guid BreedId { get; }

    }
}
