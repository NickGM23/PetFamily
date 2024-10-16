
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.EntityIds;

namespace PetFamily.VolunteerManagement.Domain.ValueObjects
{
    public record PetBreed
    {
        public SpeciesId SpeciesId { get; } = null!;

        public Guid BreedId { get; }

        private PetBreed()
        {
        }

        private PetBreed(SpeciesId speciesId, Guid breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public static PetBreed None => Create(SpeciesId.Empty(), Guid.Empty).Value;

        public static Result<PetBreed> Create(SpeciesId speciesId, Guid breedId)
        {
            return new PetBreed(speciesId, breedId);
        }
    }
}
