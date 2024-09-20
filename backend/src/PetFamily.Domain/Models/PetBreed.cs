
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Models
{
    public record PetBreed
    {
        public SpeciesId SpeciesId { get; } = null!;

        public Guid BreedId { get; }

        private PetBreed()
        {
        }

        private PetBreed(SpeciesId speciesId, BreedId breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public static Result<PetBreed> Create(SpeciesId speciesId, BreedId breedId)
        {
            return new PetBreed(speciesId, breedId);
        }
    }
}
